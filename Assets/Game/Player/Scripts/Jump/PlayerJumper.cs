using ReaaliStudio.Systems.ScriptableValue;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumper : MonoBehaviour, IPlayerBehaviour
{
    public bool IsJumping => _isJumping;
    public bool ReachedApex => _reachedApex;
    public bool Grounded => _grounded;
    public bool IsChargingJump => _isChargingJump;
    public PlayerFacade Facade { get; set; }
    public Propulsor InRangePropulsor { get; private set; }

    public Rigidbody2D movementRb;
    public Transform scaleTransform;
    public Transform rotationTransform;

    [SerializeField] private InputReader inputReader;
    [SerializeField] private FloatValue normalizedJumpStrength;
    [SerializeField] private FloatValue pressTimeForMaxJump;
    [SerializeField] private Vector2Value propulsionDirection;
    [SerializeField] private float inputBufferTime = .15f;
    [SerializeField] private List<JumpEffect> jumpEffects;
    [SerializeField] private LayerMask groundLayers;

    private bool _isJumping;
    private bool _isChargingJump;
    private bool _reachedApex;
    private bool _grounded;

    private float _pressingTime;
    private float _beginChargeTime;
    private bool _hasPressedJumpInput;

    private Vector3 _previousPosition;
    private bool _hasPressedPropulseInput;


    #region Unity Events

    private void Start()
    {
        _previousPosition = movementRb.linearVelocity;
        _isChargingJump = false;
        _isJumping = false;
        _grounded = false;
        _reachedApex = false;

        inputReader.JumpBeginEvent += OnJumpInputPressed;
        inputReader.JumpReleaseEvent += OnJumpInputReleased;
        inputReader.PropulsionBeginEvent += OnPropulsionInputPressed;
        inputReader.PropulsionReleaseEvent += OnPropulsionInputReleased;
        inputReader.MoveEvent += OrientPropulsion;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInputPressed();
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnJumpInputReleased();
        }
    }
    
    private void FixedUpdate()
    {
        foreach (var effect in jumpEffects)
        {
            effect.OnFixedUpdate(this);
        }

        if(IsJumping)
        {
            Vector2 dir = movementRb.position - (Vector2)_previousPosition;

            if (dir.y < 0f && !_reachedApex)
            {
                _reachedApex = true;
                foreach (var effect in jumpEffects)
                {
                    effect.ApexReached(this);
                }
            }

            _previousPosition = movementRb.position;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if ((groundLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            _grounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((groundLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            _isJumping = false;
            _grounded = true;

            foreach (var effect in jumpEffects)
            {
                effect.Landed(this);
            }

            if (_hasPressedJumpInput && Time.time - _pressingTime <= inputBufferTime)
            {
                BeginJumpCharge();
            }
        }
    }

    #endregion

    #region Jump Methods

    private void OnJumpInputPressed()
    {
        _pressingTime = Time.time;
        _hasPressedJumpInput = true;
        BeginJumpCharge();
    }

    private void OnJumpInputReleased()
    {
        _hasPressedJumpInput = false;

        if (!_isChargingJump)
        {
            return;
        }
            
        _isChargingJump = false;
        _isJumping = true;
        _reachedApex = false;
        normalizedJumpStrength.Value = Mathf.Clamp01((Time.time - _beginChargeTime) / pressTimeForMaxJump.Value);

        foreach (var effect in jumpEffects)
        {
            effect.Jump(this);
        }
    }

    private void BeginJumpCharge()
    {
        if (_isJumping)
        {
            return;
        }
            
        _beginChargeTime = Time.time;
        _isChargingJump = true;
        _reachedApex = false;
        normalizedJumpStrength.Value = 0f;
        _previousPosition = Vector3.zero;

        foreach (var effect in jumpEffects)
        {
            effect.ChargingJump(this);
        }
    }

    #endregion

    #region Propulsion Methods
    
    public void OnEnterPropulsor(Propulsor propulsor)
    {
        InRangePropulsor = propulsor;
    }

    public void OnExitPropulsor(Propulsor propulsor)
    {
        InRangePropulsor = null;
    }
    
    private void OnPropulsionInputPressed()
    {
        if (!InRangePropulsor)
        {
            return;
        }

        _previousPosition = Vector3.zero;
        _hasPressedPropulseInput = true;
        
        foreach (var effect in jumpEffects)
        {
            effect.ChargingPropulsion(this);
        }
    }

    private void OnPropulsionInputReleased()
    {
        if (!_hasPressedPropulseInput)
        {
            return;
        }

        _hasPressedPropulseInput = false;
        _reachedApex = false;
        _previousPosition = Vector3.zero;
        
        foreach (var effect in jumpEffects)
        {
            effect.Propulse(this);
        }
    }

    private void OrientPropulsion(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            propulsionDirection.Value = Vector2.up;
            return;
        }
        
        propulsionDirection.Value = direction.normalized;
    }

    #endregion
}

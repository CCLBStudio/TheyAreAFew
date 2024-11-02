using ReaaliStudio.Systems.ScriptableValue;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumper : MonoBehaviour
{
    public bool IsJumping => _isJumping;
    public bool ReachedApex => _reachedApex;
    public bool Grounded => _grounded;
    public bool IsChargingJump => _isChargingJump;

    public Rigidbody2D movementRb;
    public Transform scaleTransform;
    public Transform rotationTransform;

    [SerializeField] private InputReader inputReader;
    [SerializeField] private FloatValue normalizedJumpStrength;
    [SerializeField] private FloatValue pressTimeForMaxJump;
    [SerializeField] private FloatValue groundedGravityScale;
    [SerializeField] private float inputBufferTime = .15f;
    [SerializeField] private List<JumpEffect> jumpEffects;
    [SerializeField] private LayerMask groundLayers;

    private bool _isJumping;
    private bool _isChargingJump;
    private bool _reachedApex;
    private bool _grounded;

    private float _pressingTime;
    private float _beginChargeTime;
    private bool _hasPressedInput;

    private Vector3 _previousPosition;

    private void Start()
    {
        _previousPosition = movementRb.linearVelocity;
        _isChargingJump = false;
        _isJumping = false;
        _grounded = false;
        _reachedApex = false;

        inputReader.JumpBeginEvent += OnJumpInputPressed;
        inputReader.JumpReleaseEvent += OnJumpInputReleased;
    }

    private void OnJumpInputPressed()
    {
        _pressingTime = Time.time;
        _hasPressedInput = true;
        BeginJumpCharge();
    }

    private void OnJumpInputReleased()
    {
        _hasPressedInput = false;

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
            movementRb.gravityScale = groundedGravityScale.Value;

            foreach (var effect in jumpEffects)
            {
                effect.Landed(this);
            }

            if (_hasPressedInput && Time.time - _pressingTime <= inputBufferTime)
            {
                BeginJumpCharge();
            }
        }
    }
}

using ReaaliStudio.Systems.ScriptableValue;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public bool IsJumping => _isJumping;
    
    public Rigidbody movementRb;
    public Transform scaleTransform;
    public Transform rotationTransform;

    [SerializeField] private FloatValue normalizedJumpStrength;
    [SerializeField] private FloatValue pressTimeForMaxJump;
    [SerializeField] private List<JumpEffect> jumpEffects;

    private bool _isJumping;
    private bool _isChargingJump;
    private bool _reachedApex;
    private bool _grounded;

    private float _pressingTime;

    private Vector3 _previousVelocity;

    private void Start()
    {
        _previousVelocity = movementRb.linearVelocity;
        _isChargingJump = false;
        _isJumping = false;
        _grounded = false;
        _reachedApex = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isJumping)
        {
            _isChargingJump = true;
            _reachedApex = false;
            normalizedJumpStrength.Value = 0f;
            _pressingTime = Time.time;
            _previousVelocity = Vector3.zero;

            foreach (var effect in jumpEffects)
            {
                effect.ChargingJump(this);
            }
        }
        
        if (Input.GetKeyUp(KeyCode.Space) && _isChargingJump)
        {
            _isChargingJump = false;
            _isJumping = true;
            _reachedApex = false;
            normalizedJumpStrength.Value = Mathf.Clamp01((Time.time - _pressingTime) / pressTimeForMaxJump.Value);

            foreach (var effect in jumpEffects)
            {
                effect.Jump(this);
            }
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
            Vector3 dir = (movementRb.position - _previousVelocity);

            if (dir.y < 0f && !_reachedApex)
            {
                _reachedApex = true;
                foreach (var effect in jumpEffects)
                {
                    effect.ApexReached(this);
                }
            }

            _previousVelocity = movementRb.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isJumping = false;
        _grounded = true;

        foreach (var effect in jumpEffects)
        {
            effect.Landed(this);
        }
    }
}

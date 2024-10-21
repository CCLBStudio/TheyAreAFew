using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public Rigidbody rb;
    public Transform scaleTransform;

    [SerializeField] private List<JumpEffect> jumpEffects;

    private bool _isJumping;
    private bool _isChargingJump;
    private bool _reachedApex;
    private bool _grounded;

    private Vector3 _previousVelocity;

    private void Start()
    {
        _previousVelocity = rb.linearVelocity;
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

        Vector3 dir = (rb.linearVelocity - _previousVelocity).normalized;
        if(dir.y < 0f && _isJumping && !_reachedApex)
        {
            _reachedApex = true;
            foreach (var effect in jumpEffects)
            {
                effect.ApexReached(this);
            }
        }

        _previousVelocity = rb.linearVelocity;
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

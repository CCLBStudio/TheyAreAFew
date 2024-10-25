using System;
using ReaaliStudio.Systems.ScriptableValue;
using UnityEngine;

[CreateAssetMenu(menuName = "They Are Many/Player/Jump/Effects/Additional Gravity Jump Effect", fileName = "NewAdditionalGravityJumpEffect")]
public class GravityModifierJumpEffect : JumpEffect
{
    [SerializeField] private float upwardGravityScale = 1f;
    [SerializeField] private float downwardGravityScale = 1f;
    [SerializeField] private float apexGravityTime = .2f;
    [SerializeField] private float maximumFallingVelocity = 1f;
    [SerializeField] [Range(0f, 1f)] private float normalizedJumpStrengthOffset = .4f;
    [SerializeField] private float verticalVelocityForApexGravitySmoothing = 3f;
    [SerializeField] private FloatValue normalizedJumpStrength;
    [SerializeField] private FloatValue groundedGravityScale;
    
    [NonSerialized] private bool _isDoingAntiGravityApex;
    [NonSerialized] private float _apexReachedTime;
    [NonSerialized] private float _upwardGravityScale;

    public override void ApexReached(PlayerJump jumper)
    {
        _apexReachedTime = Time.time;
        jumper.movementRb.gravityScale = 0f;
        jumper.movementRb.linearVelocity = Vector2.zero;
        _isDoingAntiGravityApex = true;
    }

    public override void ChargingJump(PlayerJump jumper)
    {

    }

    public override void Jump(PlayerJump jumper)
    {
        _isDoingAntiGravityApex = false;
        _upwardGravityScale = Mathf.Clamp01(normalizedJumpStrength.Value + normalizedJumpStrengthOffset) * (groundedGravityScale.Value + (upwardGravityScale - groundedGravityScale.Value));
        jumper.movementRb.gravityScale = _upwardGravityScale;
    }

    public override void Landed(PlayerJump jumper)
    {
    }

    public override void OnFixedUpdate(PlayerJump jumper)
    {
        if (!jumper.IsJumping)
        {
            return;
        }
        
        if (!jumper.ReachedApex && jumper.movementRb.linearVelocityY <= verticalVelocityForApexGravitySmoothing)
        {
            jumper.movementRb.gravityScale = Mathf.Clamp(jumper.movementRb.linearVelocityY / verticalVelocityForApexGravitySmoothing * _upwardGravityScale, .15f, _upwardGravityScale);
        }
        else if (jumper.ReachedApex && _isDoingAntiGravityApex && Time.time - _apexReachedTime >= apexGravityTime * normalizedJumpStrength.Value)
        {
            _isDoingAntiGravityApex = false;
            jumper.movementRb.gravityScale = downwardGravityScale;
        }
        else if(_isDoingAntiGravityApex)
        {
            jumper.movementRb.linearVelocity = Vector2.zero;
        }
        else if (jumper.ReachedApex && !jumper.Grounded)
        {
            jumper.movementRb.linearVelocityY = Mathf.Max(jumper.movementRb.linearVelocityY, maximumFallingVelocity);
        }
    }
}

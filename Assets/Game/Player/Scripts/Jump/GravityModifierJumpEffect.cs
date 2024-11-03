using System;
using PrimeTween;
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
    [NonSerialized] private bool _isChargingPropulsion;
    [NonSerialized] private float _apexReachedTime;

    private Vector2 _additionalForceReference;

    public override void ChargingJump(PlayerJumper jumper)
    {

    }

    public override void Jump(PlayerJumper jumper)
    {
        _isDoingAntiGravityApex = false;
        float scale = Mathf.Clamp01(normalizedJumpStrength.Value + normalizedJumpStrengthOffset) * (groundedGravityScale.Value + (upwardGravityScale - groundedGravityScale.Value));
        _additionalForceReference = Physics2D.gravity * scale;
    }
    
    public override void ApexReached(PlayerJumper jumper)
    {
        _apexReachedTime = Time.time;
        _isDoingAntiGravityApex = true;
    }

    public override void Landed(PlayerJumper jumper)
    {
    }

    public override void OnFixedUpdate(PlayerJumper jumper)
    {
        if (_isChargingPropulsion)
        {
            return;
        }
        
        if (!jumper.ReachedApex)
        {
            var force = _additionalForceReference * Mathf.Clamp01(jumper.movementRb.linearVelocityY / verticalVelocityForApexGravitySmoothing);
            jumper.movementRb.AddForce(force);
        }
        else if (_isDoingAntiGravityApex && Time.time - _apexReachedTime >= apexGravityTime * normalizedJumpStrength.Value)
        {
            _isDoingAntiGravityApex = false;
            Tween.Custom(Vector2.zero, Physics2D.gravity * downwardGravityScale, .25f, vector2 => _additionalForceReference = vector2);
        }
        
        else if(_isDoingAntiGravityApex)
        {
            jumper.movementRb.linearVelocityY = 0f;
        }
        else if (jumper.ReachedApex && !jumper.Grounded)
        {
            jumper.movementRb.AddForce(_additionalForceReference);
            jumper.movementRb.linearVelocityY = Mathf.Max(jumper.movementRb.linearVelocityY, maximumFallingVelocity);
        }
    }

    public override void ChargingPropulsion(PlayerJumper jumper)
    {
        _isChargingPropulsion = true;
        _isDoingAntiGravityApex = false;
    }

    public override void Propulse(PlayerJumper jumper)
    {
        _isChargingPropulsion = false;
    }
}


using PrimeTween;
using ReaaliStudio.Systems.ScriptableValue;
using UnityEngine;

[CreateAssetMenu(menuName = "They Are Many/Player/Jump/Effects/Add Force Jump Effect", fileName = "NewAddForceJumpEffect")]
public class AddForceJumpEffect : JumpEffect
{
    [SerializeField] private float maxJumpForce = 25f;
    [SerializeField] private float propulsionForce = 25f;
    [SerializeField] private FloatValue normalizedJumpStrength;
    [SerializeField] private Vector2Value propulsionDirection;

    private bool _jumpInputReleased;
    private bool _propulsionInputReleased;
    private Tween _propulsionVelocitySmoother;
    private Vector2 _propulsionDir;
    
    public override void ChargingJump(PlayerJumper jumper)
    {
        _jumpInputReleased = false;
    }

    public override void Jump(PlayerJumper jumper)
    {
        _jumpInputReleased = true;
    }

    public override void ApexReached(PlayerJumper jumper)
    {
    }

    public override void Landed(PlayerJumper jumper)
    {
    }
    
    public override void OnFixedUpdate(PlayerJumper jumper)
    {
        if (_jumpInputReleased)
        {
            _jumpInputReleased = false;
            jumper.movementRb.AddForce(Vector2.up * (maxJumpForce * normalizedJumpStrength.Value), ForceMode2D.Impulse);
        }

        if (_propulsionInputReleased)
        {
            _propulsionInputReleased = false;
            if (jumper.InRangePropulsor)
            {
                jumper.movementRb.AddForce(_propulsionDir * propulsionForce, ForceMode2D.Impulse);
            }
        }
    }

    public override void ChargingPropulsion(PlayerJumper jumper)
    {
        _propulsionVelocitySmoother = Tween.Custom(jumper.movementRb.linearVelocity, Vector2.zero, .2f, vector2 => jumper.movementRb.linearVelocity = vector2, Ease.OutSine);
    }

    public override void Propulse(PlayerJumper jumper)
    {
        if (_propulsionVelocitySmoother.isAlive)
        {
            _propulsionVelocitySmoother.Stop();
        }

        _propulsionInputReleased = true;
        _propulsionDir = propulsionDirection.Value;
    }
}

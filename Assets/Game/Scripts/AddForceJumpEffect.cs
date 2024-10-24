
using ReaaliStudio.Systems.ScriptableValue;
using UnityEngine;

[CreateAssetMenu(menuName = "They Are Many/Player/Jump/Effects/Add Force Jump Effect", fileName = "NewAddForceJumpEffect")]
public class AddForceJumpEffect : JumpEffect
{
    [SerializeField] private float maxJumpForce = 25f;
    [SerializeField] private FloatValue normalizedJumpStrength;

    private bool _jumpInputReleased;
    
    public override void ChargingJump(PlayerJump jumper)
    {
        _jumpInputReleased = false;
    }

    public override void Jump(PlayerJump jumper)
    {
        _jumpInputReleased = true;
    }

    public override void ApexReached(PlayerJump jumper)
    {
    }

    public override void Landed(PlayerJump jumper)
    {
    }
    
    public override void OnFixedUpdate(PlayerJump jumper)
    {
        if (_jumpInputReleased)
        {
            _jumpInputReleased = false;
            jumper.movementRb.AddForce(Vector2.up * (maxJumpForce * normalizedJumpStrength.Value), ForceMode2D.Impulse);
        }
    }
}

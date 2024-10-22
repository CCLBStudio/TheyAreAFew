
using UnityEngine;

[CreateAssetMenu(menuName = "They Are Many/Player/Jump/Effects/Add Force Jump Effect", fileName = "NewAddForceJumpEffect")]
public class AddForceJumpEffect : JumpEffect
{
    [SerializeField] private float maxJumpForce = 25f;
    [SerializeField] private float pressTimeForMaxForce = 1f;

    private float _startChargingTime;
    private bool _jumpInputReleased;
    private float _normalizedJumpForce;
    
    public override void ChargingJump(PlayerJump jumper)
    {
        _startChargingTime = Time.time;
        _jumpInputReleased = false;
    }

    public override void Jump(PlayerJump jumper)
    {
        float time = Time.time;
        float deltaTime = time - _startChargingTime;
        _normalizedJumpForce = Mathf.Clamp(deltaTime / pressTimeForMaxForce, 0f, 1f);
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
            jumper.movementRb.AddForce(Vector3.up * (maxJumpForce * _normalizedJumpForce), ForceMode.Impulse);
        }
    }
}

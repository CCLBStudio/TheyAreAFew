using UnityEngine;

[CreateAssetMenu(menuName = "They Are Many/Player/Jump/Effects/Add Torque Jump Effect", fileName = "NewAddTorqueJumpEffect")]
public class AddTorqueJumpEffect : JumpEffect
{
    [SerializeField] private float torqueStrength = 10f;
    
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
            Vector3 torque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            jumper.rb.AddTorque(torque * torqueStrength);
            _jumpInputReleased = false;
        }
    }
}

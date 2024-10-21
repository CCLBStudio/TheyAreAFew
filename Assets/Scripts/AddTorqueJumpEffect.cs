using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "They Are Many/Player/Jump/Effects/Add Torque Jump Effect", fileName = "NewAddTorqueJumpEffect")]
public class AddTorqueJumpEffect : JumpEffect
{
    [SerializeField] private float torqueStrength = 10f;
    [SerializeField] private float angularDragDuringJump = 10f;
    
    [NonSerialized] private bool _jumpInputReleased;
    [NonSerialized] private float _angularDragBeforeJump;
    
    public override void ChargingJump(PlayerJump jumper)
    {
        _jumpInputReleased = false;
        _angularDragBeforeJump = jumper.rb.angularDamping;
    }

    public override void Jump(PlayerJump jumper)
    {
        _jumpInputReleased = true;
        jumper.rb.angularDamping = angularDragDuringJump;
    }

    public override void ApexReached(PlayerJump jumper)
    {
    }

    public override void Landed(PlayerJump jumper)
    {
        jumper.rb.angularDamping = _angularDragBeforeJump;
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

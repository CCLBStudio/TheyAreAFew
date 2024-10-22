using System;
using UnityEngine;

[CreateAssetMenu(menuName = "They Are Many/Player/Jump/Effects/Additional Gravity Jump Effect", fileName = "NewAdditionalGravityJumpEffect")]
public class AdditionalGravityJumpEffect : JumpEffect
{
    [SerializeField] private float upwardGravityScale = 1f;
    [SerializeField] private float downwardGravityScale = 1f;

    [NonSerialized] private bool _goingUpward;
    [NonSerialized] private bool _goingDownard;

    public override void ApexReached(PlayerJump jumper)
    {
        _goingUpward = false;
        _goingDownard = true;
    }

    public override void ChargingJump(PlayerJump jumper)
    {
        _goingDownard = false;
        _goingUpward = false;
    }

    public override void Jump(PlayerJump jumper)
    {
        _goingUpward = true;
    }

    public override void Landed(PlayerJump jumper)
    {
        _goingDownard = false;
        _goingUpward = true;
    }

    public override void OnFixedUpdate(PlayerJump jumper)
    {
        if(_goingUpward)
        {
            jumper.movementRb.AddForce(Physics.gravity * ((upwardGravityScale - 1f)));
        }
        else if(_goingDownard)
        {
            jumper.movementRb.AddForce(Physics.gravity * ((downwardGravityScale - 1f)));
        }
    }
}

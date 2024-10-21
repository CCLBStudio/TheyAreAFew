using UnityEngine;

public abstract class JumpEffect : ScriptableObject
{
    public abstract void ChargingJump();
    public abstract void Jump();
    public abstract void MovingUpwards();
    public abstract void ApexReached();
    public abstract void MovingDownwards();
    public abstract void Landed();
}

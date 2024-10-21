using UnityEngine;

public abstract class JumpEffect : ScriptableObject
{
    public abstract void ChargingJump(PlayerJump jumper);
    public abstract void Jump(PlayerJump jumper);
    public abstract void MovingUpwards(PlayerJump jumper);
    public abstract void ApexReached(PlayerJump jumper);
    public abstract void MovingDownwards(PlayerJump jumper);
    public abstract void Landed(PlayerJump jumper);
}

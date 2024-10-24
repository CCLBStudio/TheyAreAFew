using UnityEngine;

public abstract class JumpEffect : ScriptableObject
{
    public abstract void ChargingJump(PlayerJump jumper);
    public abstract void Jump(PlayerJump jumper);
    public abstract void ApexReached(PlayerJump jumper);
    public abstract void Landed(PlayerJump jumper);
    public abstract void OnFixedUpdate(PlayerJump jumper);
}

using UnityEngine;

public abstract class JumpEffect : ScriptableObject
{
    public abstract void ChargingJump(PlayerJumper jumper);
    public abstract void Jump(PlayerJumper jumper);
    public abstract void ApexReached(PlayerJumper jumper);
    public abstract void Landed(PlayerJumper jumper);
    public abstract void OnFixedUpdate(PlayerJumper jumper);
}

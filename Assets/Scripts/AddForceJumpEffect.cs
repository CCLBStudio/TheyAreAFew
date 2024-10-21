
using UnityEngine;

[CreateAssetMenu(menuName = "They Are Many/Player/Jump/Effects/Add Force Jump Effect", fileName = "NewAddForceJumpEffect")]
public class AddForceJumpEffect : JumpEffect
{
    [SerializeField] private float maxJumpForce = 25f;
    [SerializeField] private float pressTimeForMaxForce = 1f;

    private float _startChargingTime;
    
    public override void ChargingJump(PlayerJump jumper)
    {
        Debug.Log("Charging jump !");
        _startChargingTime = Time.time;
    }

    public override void Jump(PlayerJump jumper)
    {
        Debug.Log("Jump !");

        float time = Time.time;
        float deltaTime = time - _startChargingTime;
        float normalized = Mathf.Clamp(deltaTime / pressTimeForMaxForce, 0f, 1f);
        jumper.rb.AddForce(Vector3.up * (maxJumpForce * normalized), ForceMode.Impulse);
    }

    public override void MovingUpwards(PlayerJump jumper)
    {
    }

    public override void ApexReached(PlayerJump jumper)
    {
    }

    public override void MovingDownwards(PlayerJump jumper)
    {
    }

    public override void Landed(PlayerJump jumper)
    {
    }
}

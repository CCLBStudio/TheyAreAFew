using PrimeTween;
using UnityEngine;

[CreateAssetMenu(menuName = "They Are Many/Player/Jump/Effects/Add Torque Jump Effect", fileName = "NewAddTorqueJumpEffect")]
public class RotationJumpEffect : JumpEffect
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private int maxTurns = 3;
    
    public override void ChargingJump(PlayerJump jumper)
    {
    }

    public override void Jump(PlayerJump jumper)
    {
        int turns = Random.Range(1, maxTurns + 1);
        Vector3 rotAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        Tween.Custom(0f, 360f * turns, duration, angle => jumper.rotationTransform.rotation = Quaternion.AngleAxis(angle, rotAxis), Ease.OutCubic);
    }

    public override void ApexReached(PlayerJump jumper)
    {
    }

    public override void Landed(PlayerJump jumper)
    {
    }

    public override void OnFixedUpdate(PlayerJump jumper)
    {
    }
}

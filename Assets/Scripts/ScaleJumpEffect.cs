using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "They Are Many/Player/Jump/Effects/Scale Jump Effect", fileName = "NewScale JumpEffect")]
public class ScaleJumpEffect : JumpEffect
{
    [SerializeField] private float chargingTime = .3f;
    [SerializeField] private float resetTime = .3f;
    [Range(0f, 1f)][SerializeField] private float minScaleSize = .2f;

    private Tween _currentTween;

    public override void ApexReached(PlayerJump jumper)
    {
    }

    public override void ChargingJump(PlayerJump jumper)
    {
        _currentTween = jumper.scaleTransform.DOScaleY(minScaleSize, chargingTime).SetEase(Ease.Linear)
    }

    public override void Jump(PlayerJump jumper)
    {
        _currentTween.Kill();
        _currentTween = jumper.scaleTransform.DOScaleY(1f, resetTime).SetEase(Ease.OutBack);
    }

    public override void Landed(PlayerJump jumper)
    {
        jumper.scaleTransform.localScale = Vector3.one;
    }

    public override void OnFixedUpdate(PlayerJump jumper)
    {
    }
}

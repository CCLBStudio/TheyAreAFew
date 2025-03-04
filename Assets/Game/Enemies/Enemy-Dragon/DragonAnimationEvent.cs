using CCLBStudio.ScriptablePooling;
using UnityEngine;
using UnityEngine.Events;

public class DragonAnimationEvent : MonoBehaviour
{
    public UnityEvent onFireAttackBegin;

    [SerializeField] private ScriptablePool fireballPool;
    [SerializeField] private Transform fireballOrigin;
    
    public void OnFireAttackBegin()
    {
        LaunchFireball();
        onFireAttackBegin?.Invoke();
    }

    private void LaunchFireball()
    {
        var ball = fireballPool.RequestObjectAs<DragonFireball>();
        ball.transform.SetPositionAndRotation(fireballOrigin.position, fireballOrigin.rotation);
        // ball.transform.position = fireballOrigin.position;
        // ball.transform.rotation = fireballOrigin.rotation;
    }
}

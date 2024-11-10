using CCLBStudio.ScriptablePooling;
using PrimeTween;
using UnityEngine;

public class PooledCasing : MonoBehaviour, IScriptablePooledObject
{
    public ScriptablePool Pool { get; set; }
    public Rigidbody Rigidbody => rb;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float gravityScale = 5f;
    [SerializeField] private float disappearTime = 1f;

    private float _timer;
    private Vector3 _initialScale;

    private void FixedUpdate()
    {
        if (_timer <= 0f)
        {
            return;
        }
        
        _timer -= Time.fixedDeltaTime;
        if (_timer <= 0f)
        {
            SmoothRelease();
            return;
        }
        
        rb.AddForce(Physics.gravity * gravityScale);
    }

    private void SmoothRelease()
    {
        Tween.Scale(transform, 0f, disappearTime, Ease.OutSine).OnComplete(ReleaseSelf);
    }

    private void ReleaseSelf()
    {
        Pool.ReleaseObject(this);
    }

    public void OnObjectCreated()
    {
        _initialScale = transform.localScale;
        _timer = lifetime;
    }

    public void OnObjectRequested()
    {
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        transform.localScale = _initialScale;
        _timer = lifetime;
    }

    public void OnObjectReleased()
    {
    }
}

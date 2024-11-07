using PrimeTween;
using UnityEngine;

public class PooledRocket : PooledAbilityObject<ScriptableRocketAbility>
{
    [SerializeField] private Collider2D rocketCollider;
    [SerializeField] private Rigidbody2D rb;
    
    private float _currentSpeed;
    private Tween _accelerationTween;

    public override void Initialize()
    {
        float maxSpeed = scriptableAbility.TravelSpeed;
        _accelerationTween = Tween.Custom(0f, maxSpeed, scriptableAbility.AccelerationTime, f => _currentSpeed = f, scriptableAbility.AccelerationEase);
        
        rocketCollider.enabled = true;
    }

    private void FixedUpdate()
    {
        Vector2 right = transform.right;
        rb.MovePosition(rb.position + right * (_currentSpeed * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_accelerationTween.isAlive)
        {
            _accelerationTween.Stop();
        }
        
        Pool.ReleaseObject(this);
    }

    public override void OnObjectCreated()
    {
        rocketCollider.enabled = false;
    }

    public override void OnObjectRequested()
    {
    }
    
    public override void OnObjectReleased()
    {
        rocketCollider.enabled = false;
    }
}

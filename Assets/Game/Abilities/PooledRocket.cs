using PrimeTween;
using UnityEngine;

public class PooledRocket : PooledAbilityObject<ScriptableRocketAbility>, IDamageDealer
{
    [SerializeField] private Collider2D rocketCollider;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask collisionMask;
    
    private float _currentSpeed;
    private Tween _accelerationTween;
    private bool _isAlive;

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
        if (!_isAlive)
        {
            return;
        }
        _isAlive = false;

        if (_accelerationTween.isAlive)
        {
            _accelerationTween.Stop();
        }

        var effect = scriptableAbility.ExplosionPool.RequestObjectAs<PooledParticleSystem>();
        Vector3 abilityPos = transform.position;
        effect.transform.position = abilityPos;
        effect.Play();

        ApplyDamages();
        Pool.ReleaseObject(this);
    }

    private void ApplyDamages()
    {
        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, scriptableAbility.ExplosionRange, collisionMask);
        foreach (var col in inRange)
        {
            if (!col.gameObject.TryGetComponent(out DamageInteractor i))
            {
                continue;
            }

            i.GetHit(this);
        }
    }

    public override void OnObjectCreated()
    {
        rocketCollider.enabled = false;
        _isAlive = false;
    }

    public override void OnObjectRequested()
    {
        _isAlive = true;
    }
    
    public override void OnObjectReleased()
    {
        rocketCollider.enabled = false;
    }
    
    public float GetDamages()
    {
        return scriptableAbility.Strength;
    }

    public void ApplyKnockback(IDamageable target)
    {
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, scriptableAbility.ExplosionRange);
    }

#endif
}

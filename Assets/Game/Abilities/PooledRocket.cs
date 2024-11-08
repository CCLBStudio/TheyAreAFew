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
    private float _lifetime;

    public override void Initialize()
    {
        float maxSpeed = scriptableAbility.TravelSpeed;
        _accelerationTween = Tween.Custom(0f, maxSpeed, scriptableAbility.AccelerationTime, f => _currentSpeed = f, scriptableAbility.AccelerationEase);
        
        rocketCollider.enabled = true;
    }

    private void FixedUpdate()
    {
        _lifetime -= Time.fixedDeltaTime;
        if (_lifetime <= 0f)
        {
            Pool.ReleaseObject(this);
            return;
        }
        
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
        
        PlayExplosionParticles();
        ApplyDamages();
        Pool.ReleaseObject(this);
    }

    private void PlayExplosionParticles()
    {
        var effect = scriptableAbility.ExplosionPool.RequestObjectAs<PooledParticleSystem>();
        Vector3 abilityPos = transform.position;
        effect.transform.position = abilityPos;
        effect.Play();
    }

    private void ApplyDamages()
    {
        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, scriptableAbility.ExplosionRange, collisionMask);
        foreach (var col in inRange)
        {
            var damageables = col.gameObject.GetComponents<IDamageable>();
            foreach (var d in damageables)
            {
                d.GetHit(this);

                if (d.GetRigidbody())
                {
                    d.GetRigidbody().AddExplosionForce(scriptableAbility.KnockbackForce, rb.position, scriptableAbility.ExplosionRange, 1f);
                }
            }
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
        _lifetime = scriptableAbility.Lifetime;
    }
    
    public override void OnObjectReleased()
    {
        rocketCollider.enabled = false;
    }

    #region Damageable Methods

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public DamageType GetDamageType()
    {
        return DamageType.Explosion;
    }

    public float GetDamages()
    {
        return scriptableAbility.Strength;
    }

    #endregion

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, scriptableAbility.ExplosionRange);
    }

#endif
}

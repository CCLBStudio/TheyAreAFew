using UnityEngine;
using UnityEngine.Events;

public class GiftHealth : MonoBehaviour, IDamageable
{
    public float health = 500;
    public UnityEvent<IDamageDealer> hitEvent;
    public UnityEvent onDeath;

    private bool _isDead = false;

    
    public void GetHit(IDamageDealer damageDealer)
    {
        if (_isDead)
        {
            return;
        }
        
        health -= damageDealer.GetDamages();
        hitEvent?.Invoke(damageDealer);

        if (health <= 0f)
        {
            _isDead = true;
            onDeath?.Invoke();
        }
        
    }

    public Rigidbody2D GetRigidbody()
    {
        return null;
    }
}

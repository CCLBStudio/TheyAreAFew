using UnityEngine;
using UnityEngine.Events;

public class DamageableEventBinder : MonoBehaviour, IDamageable
{
    public UnityEvent<IDamageDealer> hitEvent;
    public void GetHit(IDamageDealer damageOrigin)
    {
        hitEvent?.Invoke(damageOrigin);
    }

    public Rigidbody2D GetRigidbody()
    {
        return null;
    }
}

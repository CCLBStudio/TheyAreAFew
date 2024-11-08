using UnityEngine;

public class KnockbackInfosHolder : MonoBehaviour, IDamageable
{
    [SerializeField] private Rigidbody2D rb;
    public void GetHit(IDamageDealer damageDealer)
    {
        
    }

    public Rigidbody2D GetRigidbody()
    {
        return rb;
    }
}

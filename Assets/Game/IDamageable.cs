using UnityEngine;

public interface IDamageable
{
    public void GetHit(IDamageDealer damageDealer);
    public Rigidbody2D GetRigidbody();
}

using UnityEngine;

public interface IDamageDealer
{
    public Vector3 GetPosition();
    public DamageType GetDamageType();
    public float GetDamages();
}

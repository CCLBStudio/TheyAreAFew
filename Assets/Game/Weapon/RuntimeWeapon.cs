using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class RuntimeWeapon : MonoBehaviour
{
    public float AttackSpeed => GetAttackSpeed();
    
    [SerializeField] private ScriptableWeapon weapon;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private Transform casingOrigin;

    [SerializeField] private Vector2 casingEjectionDirection = new Vector2(-1f, 2f).normalized;
    
    public void Initialize(PlayerAttack attacker)
    {
        weapon.BulletPool.Initialize();
    }

    public void Shoot(Vector2 direction)
    {
        var bullet = weapon.BulletPool.RequestObjectAs<RuntimeBullet>();
        float dispersion = Random.Range(-weapon.Dispersion, weapon.Dispersion);
        var orientation = Quaternion.FromToRotation(Vector3.right, direction) * Quaternion.AngleAxis(dispersion, Vector3.forward);
        bullet.Direction = ApplyRotation(direction, dispersion);
        bullet.transform.SetPositionAndRotation(bulletOrigin.position, orientation);
        
        SpawnCasing(direction);
    }

    private Vector2 ApplyRotation(Vector2 direction, float dispersion)
    {
        float rad = dispersion * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        
        return new Vector2(cos * direction.x - sin * direction.y, sin * direction.x + cos * direction.y);
    }

    private void SpawnCasing(Vector2 shootingDirection)
    {
        var rb = Instantiate(weapon.Casing, casingOrigin.position, quaternion.identity).GetComponent<Rigidbody2D>();
        Vector2 dir = ApplyRotation(casingEjectionDirection, Random.Range(-weapon.CasingDispersion, weapon.CasingDispersion)).normalized;
        
        rb.AddForce(dir * weapon.CasingEjectionForce, ForceMode2D.Impulse);
        rb.AddTorque(weapon.CasingEjectionForce, ForceMode2D.Impulse);
    }

    private float GetAttackSpeed()
    {
        return weapon.AttackSpeed;
    }
}

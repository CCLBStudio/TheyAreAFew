using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class RuntimeWeapon : MonoBehaviour
{
    public float AttackSpeed => GetAttackSpeed();
    
    [SerializeField] private ScriptableWeapon weapon;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private Transform muzzleOrigin;
    [SerializeField] private Transform casingOrigin;

    [SerializeField] private Vector2 casingEjectionDirection = new Vector2(-1f, 2f).normalized;

    private Rigidbody2D _playerRb;
    private PlayerAttacker _attacker;
    
    public void Initialize(PlayerAttacker attacker)
    {
        _playerRb = attacker.PlayerRb;
        _attacker = attacker;
    }

    public void Shoot(Vector2 direction)
    {
        SpawnBullet(direction);
        SpawnMuzzle();
        ApplyKnockbackForce(direction);
        SpawnCasing();
    }

    private void SpawnBullet(Vector2 shootingDirection)
    {
        var bullet = weapon.BulletPool.RequestObjectAs<RuntimeBullet>();
        bullet.Initialize(weapon);
        
        float dispersion = Random.Range(-weapon.Dispersion, weapon.Dispersion);
        var orientation = Quaternion.FromToRotation(Vector3.right, shootingDirection) * Quaternion.AngleAxis(dispersion, Vector3.forward);
        bullet.Direction = ApplyRotation(shootingDirection, dispersion);
        bullet.transform.SetPositionAndRotation(bulletOrigin.position, orientation);
    }

    private void SpawnMuzzle()
    {
        var muzzle = weapon.MuzzlePool.RequestObjectAs<PooledMuzzle>();
        muzzle.transform.SetParent(muzzleOrigin);
        muzzle.transform.localPosition = Vector3.zero;
        muzzle.transform.localRotation = quaternion.identity;
    }

    private void ApplyKnockbackForce(Vector2 shootingDirection)
    {
        Vector2 dir = ApplyRotation(shootingDirection, 180f).normalized;
        dir.x *= _attacker.Jumper.Grounded ? 1f : weapon.PlayerInAirKnockbackForceXMultiplier;
        
        _playerRb.AddForce(dir * weapon.PlayerKnockbackForce, ForceMode2D.Impulse);
    }

    private void SpawnCasing()
    {
        var rb = Instantiate(weapon.Casing, casingOrigin.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        Vector2 dir = ApplyRotation(casingEjectionDirection, Random.Range(-weapon.CasingDispersion, weapon.CasingDispersion)).normalized;
        
        rb.AddForce(dir * weapon.CasingEjectionForce, ForceMode2D.Impulse);
        rb.AddTorque(weapon.CasingEjectionForce, ForceMode2D.Impulse);
    }
    
    private Vector2 ApplyRotation(Vector2 direction, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        
        return new Vector2(cos * direction.x - sin * direction.y, sin * direction.x + cos * direction.y);
    }

    private float GetAttackSpeed()
    {
        return weapon.AttackSpeed;
    }
}

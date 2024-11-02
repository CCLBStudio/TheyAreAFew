using System;
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

    [SerializeField] private Vector3 casingEjectionDirection = new Vector3(-1f, 2f, 0f).normalized;

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
        var muzzle = weapon.MuzzlePool.RequestObjectAs<AutoReleasePooledObject>();
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
        var casing = weapon.CasingPool.RequestObjectAs<PooledCasing>();
        casing.transform.SetPositionAndRotation(casingOrigin.position, Random.rotation);
        Vector3 dir = ApplyRotation(casingEjectionDirection, Random.Range(-weapon.CasingDispersion, weapon.CasingDispersion)).normalized;
        
        casing.Rigidbody.AddForce(dir * weapon.CasingEjectionForce, ForceMode.Impulse);
        casing.Rigidbody.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * weapon.CasingEjectionForce, ForceMode.Impulse);
    }
    
    private Vector2 ApplyRotation(Vector2 direction, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        
        return new Vector2(cos * direction.x - sin * direction.y, sin * direction.x + cos * direction.y);
    }

    private Vector3 ApplyRotation(Vector3 direction, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        float newX = cos * direction.x - sin * direction.y;
        float newY = sin * direction.x + cos * direction.y;

        return new Vector3(newX, newY, direction.z);
    }

    private float GetAttackSpeed()
    {
        return weapon.AttackSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(casingOrigin.position, casingOrigin.position + casingEjectionDirection);
    }
}

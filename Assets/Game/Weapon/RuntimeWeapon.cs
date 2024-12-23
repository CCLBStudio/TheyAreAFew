using UnityEngine;

public class RuntimeWeapon : MonoBehaviour
{
    public float AttackSpeed => GetAttackSpeed();
    
    //[SerializeField] private ScriptableWeapon weapon;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private Transform muzzleOrigin;
    [SerializeField] private Transform casingOrigin;
    [SerializeField] private Animator weaponAnimator;

    [SerializeField] private Vector3 casingEjectionDirection = new Vector3(-1f, 2f, 0f).normalized;

    private Rigidbody2D _playerRb;
    private PlayerAttacker _attacker;
    private bool _init;
    private static readonly int Shooting = Animator.StringToHash("Shooting");
    private ScriptableWeapon _weapon;

    public void Initialize(PlayerAttacker attacker, ScriptableWeapon weapon)
    {
        weaponAnimator = GetComponent<Animator>();
        _playerRb = attacker.PlayerRb;
        _attacker = attacker;
        _weapon = weapon;

        _init = true;
    }

    public void Shoot(Vector2 direction)
    {
        if (!_init)
        {
            return;
        }
        
        SpawnBullet(direction);
        SpawnMuzzle();
        ApplyKnockbackForce(direction);
        SpawnCasing();
    }

    private void SpawnBullet(Vector2 shootingDirection)
    {
        var bullet = _weapon.BulletPool.RequestObjectAs<RuntimeBullet>();
        bullet.Initialize(_weapon);
        
        float dispersion = Random.Range(-_weapon.Dispersion, _weapon.Dispersion);
        var orientation = Quaternion.FromToRotation(Vector3.right, shootingDirection) * Quaternion.AngleAxis(dispersion, Vector3.forward);
        bullet.Direction = ApplyRotation(shootingDirection, dispersion);
        bullet.transform.SetPositionAndRotation(bulletOrigin.position, orientation);
    }

    private void SpawnMuzzle()
    {
        var muzzle = _weapon.MuzzlePool.RequestObjectAs<AutoReleasePooledObject>();
        Transform muzzleTransform = muzzle.transform;
        
        muzzleTransform.SetParent(muzzleOrigin);
        muzzleTransform.localPosition = Vector3.zero;
        muzzleTransform.localRotation = Quaternion.identity;
    }

    private void ApplyKnockbackForce(Vector2 shootingDirection)
    {
        Vector2 dir = ApplyRotation(shootingDirection, 180f).normalized;
        dir.x *= _attacker.Jumper.Grounded ? 1f : _weapon.PlayerInAirKnockbackForceXMultiplier;
        
        _playerRb.AddForce(dir * _weapon.PlayerKnockbackForce, ForceMode2D.Impulse);
    }

    private void SpawnCasing()
    {
        var casing = _weapon.CasingPool.RequestObjectAs<PooledCasing>();
        casing.transform.SetPositionAndRotation(casingOrigin.position, Random.rotation);
        Vector3 dir = ApplyRotation(casingEjectionDirection, Random.Range(-_weapon.CasingDispersion, _weapon.CasingDispersion)).normalized;
        
        casing.Rigidbody.AddForce(dir * _weapon.CasingEjectionForce, ForceMode.Impulse);
        casing.Rigidbody.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * _weapon.CasingEjectionForce, ForceMode.Impulse);
    }

    public void StartShooting()
    {
        weaponAnimator.SetBool(Shooting, true);
    }
    
    public void StopShooting()
    {
        weaponAnimator.SetBool(Shooting, false);
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
        return _weapon.AttackSpeed;
    }
}

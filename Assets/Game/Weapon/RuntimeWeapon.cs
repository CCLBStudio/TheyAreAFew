using UnityEngine;

public class RuntimeWeapon : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private ScriptableWeapon weapon;

    private bool _isShooting;
    private float _shootingTimer;
    private Vector2 _shootingDirection;
    private Transform _bulletOrigin;

    #region Unity Event

    private void Start()
    {
        inputReader.AimEvent += OnAim;
    }

    private void Update()
    {
        if(_shootingTimer > 0f)
        {
            _shootingTimer -= Time.deltaTime;
            return;
        }

        if(!_isShooting)
        {
            return;
        }

        Shoot();
    }

    #endregion

    public void Initialize(Transform bulletOrigin)
    {
        weapon.BulletPool.Initialize();
        _bulletOrigin = bulletOrigin;
    }

    private void OnAim(Vector2 direction)
    {
        _isShooting = direction != Vector2.zero;
        _shootingDirection = direction.normalized;
    }

    private void Shoot()
    {
        var bullet = weapon.BulletPool.RequestObjectAs<RuntimeBullet>();
        bullet.Direction = _shootingDirection;
        bullet.transform.SetPositionAndRotation(_bulletOrigin.position + (Vector3)_shootingDirection, Quaternion.identity);
    }
}

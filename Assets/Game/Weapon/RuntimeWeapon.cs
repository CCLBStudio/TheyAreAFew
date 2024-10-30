using UnityEngine;

public class RuntimeWeapon : MonoBehaviour
{
    [SerializeField] private ScriptableWeapon weapon;
    [SerializeField] private Transform bulletOrigin;
    
    public void Initialize(PlayerAttack attacker)
    {
        weapon.BulletPool.Initialize();
    }

    public void Shoot(Vector2 direction)
    {
        var bullet = weapon.BulletPool.RequestObjectAs<RuntimeBullet>();
        bullet.Direction = direction;
        bullet.transform.SetPositionAndRotation(bulletOrigin.position, Quaternion.identity);
    }
}

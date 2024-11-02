using UnityEngine;

public class EnemyBulletInteractor : MonoBehaviour, IBulletInteractor
{
    [SerializeField] private EnemyFacade facade;
    public void GetHit(RuntimeBullet bullet)
    {
        facade.NotifyHit(bullet);
    }
}

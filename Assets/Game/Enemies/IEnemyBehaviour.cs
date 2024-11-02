public interface IEnemyBehaviour
{
    public EnemyFacade Facade { get; set; }
    public void OnEnemyCreated();
    public void OnEnemyRequested();
    public void OnEnemyReleased();
    public void OnFixedUpdated();
    public void OnBulletHit(RuntimeBullet bullet);
}

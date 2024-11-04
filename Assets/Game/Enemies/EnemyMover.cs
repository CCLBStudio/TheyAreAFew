using ReaaliStudio.Systems.ScriptableValue;
using UnityEngine;

public class EnemyMover : MonoBehaviour, IEnemyBehaviour
{
    public EnemyFacade Facade { get; set; }

    [SerializeField] private PlayerFacadeListValue players;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 minMaxSpeed = Vector2.one;

    private float _speed;
    private Vector2 _direction;

    protected virtual void Move()
    {
        _direction = target.position - transform.position;
        _direction.y = 0f;
        
        rb.AddForceX(_direction.normalized.x * _speed);
    }
    
    public void OnEnemyCreated()
    {
        _speed = Random.Range(minMaxSpeed.x, minMaxSpeed.y);
    }

    public void OnEnemyRequested()
    {
        target = players.Value[Random.Range(0, players.Value.Count)].transform;
    }

    public void OnEnemyReleased()
    {
    }

    public void OnFixedUpdated()
    {
        Move();
    }

    public void OnBulletHit(RuntimeBullet bullet)
    {
        
    }
}

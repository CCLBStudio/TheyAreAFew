using ReaaliStudio.Systems.ScriptableValue;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMover : MonoBehaviour, IEnemyBehaviour
{
    public EnemyFacade Facade { get; set; }

    [SerializeField] private PlayerFacadeListValue players;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 minMaxSpeed = Vector2.one;
    [SerializeField] private float heightDivider = 2f;

    private float _speed;
    private Vector2 _direction;
    private Transform _target;

    protected virtual void Move()
    {
        _direction = _target.position - transform.position;
        _direction.y /= heightDivider;
        
        rb.AddForce(_direction.normalized * _speed);
        Debug.DrawLine(transform.position, (transform.position +(Vector3)_direction.normalized) * _speed);
    }

    public void ApplyKnockbackForce()
    {
        
    }
    
    public void OnEnemyCreated()
    {
        _speed = Random.Range(minMaxSpeed.x, minMaxSpeed.y);
    }

    public void OnEnemyRequested()
    {
        _target = players.Value[Random.Range(0, players.Value.Count)].transform;
    }

    public void OnEnemyReleased()
    {
    }

    public void OnFixedUpdated()
    {
        Move();
    }
}

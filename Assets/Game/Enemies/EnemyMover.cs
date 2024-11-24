using ReaaliStudio.Systems.ScriptableValue;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMover : MonoBehaviour, IEnemyBehaviour
{
    public EnemyFacade Facade { get; set; }
    public Transform Target => target;
    public bool CanMove { get; set; } = true;

    [SerializeField] protected PlayerFacadeListValue players;
    [SerializeField] protected ScriptableEnemy enemyData;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Vector2 minMaxSpeed = Vector2.one;
    [SerializeField] protected float heightDivider = 2f;

    protected float speed;
    protected Vector2 direction;
    protected Transform target;

    protected virtual void Move()
    {
        direction = target.position - transform.position;
        direction.y /= heightDivider;
        
        rb.AddForce(direction.normalized * speed);
    }

    public virtual void ApplyKnockbackForce()
    {
        
    }
    
    public virtual void OnEnemyCreated()
    {
        speed = Random.Range(minMaxSpeed.x, minMaxSpeed.y);
    }

    public virtual void OnEnemyRequested()
    {
        target = players.Value[Random.Range(0, players.Value.Count)].transform;
        CanMove = true;
    }

    public virtual void OnEnemyReleased()
    {
    }

    public virtual void OnFixedUpdated()
    {
        if (!CanMove)
        {
            return;
        }
        Move();
    }
}

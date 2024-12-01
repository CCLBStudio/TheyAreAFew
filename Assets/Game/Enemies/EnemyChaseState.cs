using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyChaseState : MonoBehaviour, IEnemyBehaviour, IEnemyState
{
    public EnemyFacade Facade { get; set; }
    public EnemyStateMachine StateMachine { get; set; }
    public bool CanMove { get; set; } = true;
    protected Transform Target => targetSelector.Target;
    protected ScriptableEnemy EnemyData => Facade.EnemyData;

    [SerializeField] protected EnemyTargetSelector targetSelector;
    [SerializeField] protected EnemyAnimator animPlayer;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Vector2 minMaxSpeed = Vector2.one;
    [SerializeField] protected float heightDivider = 2f;

    [Header("Events")]
    [SerializeField] private UnityEvent startMoving;
    [SerializeField] private UnityEvent stopMoving;

    protected float speed;
    protected Vector2 direction;

    protected virtual void Move()
    {
        direction = targetSelector.Target.position - transform.position;
        direction.y /= heightDivider;
        
        rb.AddForce(direction.normalized * speed);
    }
    
    public virtual void OnEnemyCreated()
    {
        speed = Random.Range(minMaxSpeed.x, minMaxSpeed.y);
    }

    public virtual void OnEnemyRequested()
    {
        CanMove = true;
    }

    public virtual void OnEnemyReleased()
    {
    }

    public virtual void OnFixedUpdated()
    {
    }
    
    #region State Machine Methods

    public virtual void EnterState()
    {
        CanMove = true;
        animPlayer.PlayRunAnimation();
        startMoving?.Invoke();
    }

    public virtual void UpdateState()
    {
        Move();
    }

    public virtual void ExitState()
    {
        CanMove = false;
        stopMoving?.Invoke();
    }

    public virtual EnemyStateId GetStateId()
    {
        return EnemyStateId.Chase;
    }

    #endregion
}

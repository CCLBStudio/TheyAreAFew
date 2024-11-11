using PrimeTween;
using UnityEngine;

public class FlyingEnemyAttack : MonoBehaviour, IEnemyBehaviour
{
    public EnemyFacade Facade { get; set; }

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float movementTime = 1f;
    [SerializeField] private float attackCooldown = 5f;
    
    private EnemyMover _mover;
    private bool _isAttacking;
    private float _timer;
    private Vector2 _attackOrigin;
    private Sequence _attackSequence;
    
    public void OnEnemyCreated()
    {
        _mover = GetComponent<EnemyMover>();
    }

    public void OnEnemyRequested()
    {
        if (_attackSequence.isAlive)
        {
            _attackSequence.Stop();
        }
        attackCooldown = 3f;
        _timer = 0f;
    }

    public void OnEnemyReleased()
    {
        if (_attackSequence.isAlive)
        {
            _attackSequence.Stop();
        }
    }

    public void OnFixedUpdated()
    {
        if (_timer > 0f)
        {
            _timer -= Time.fixedDeltaTime;
            return;
        }
        
        if (Mathf.Abs(_mover.Target.position.x - rb.position.x) >= 1f || _isAttacking)
        {
            return;
        }
        
        LaunchAttack();
    }

    private void LaunchAttack()
    {

        Vector2 targetPos = _mover.Target.position;
        targetPos.x = rb.position.x;
        _attackOrigin = rb.position;
        float t = Mathf.Max(movementTime / Vector2.Distance(_attackOrigin, targetPos), 1f);

        _attackSequence = Sequence.Create(Tween.RigidbodyMovePosition(rb, targetPos, t, Ease.OutQuart))
            .Chain(Tween.RigidbodyMovePosition(rb, _attackOrigin, t, Ease.InQuart))
            .OnComplete(OnAttackCompleted);

        _timer = attackCooldown + 2f * t;
        _mover.CanMove = false;
        _isAttacking = true;
    }

    private void OnAttackCompleted()
    {
        _mover.CanMove = true;
        _isAttacking = false;
    }
}

using UnityEngine;

public class DragonChaseState : EnemyChaseState
{
    private Vector2 _desiredPosition;
    
    protected override void Move()
    {
        if (ComputeTargetDistance() < EnemyData.AttackRange)
        {
            StateMachine.TransitionToState(StateMachine.AttackState);
            return;
        }
        
        ComputeDesiredPosition();

        Debug.Log("Move");
        var position = rb.position;
        Vector2 dir = (_desiredPosition - position).normalized;
        rb.MovePosition(position + dir * (speed * Time.fixedDeltaTime));
    }

    private void ComputeDesiredPosition()
    {
        _desiredPosition = Target.position;
        _desiredPosition.y = rb.position.y;
    }

    private float ComputeTargetDistance()
    {
        return Vector2.Distance(rb.position, Target.position);
    }
}

using UnityEngine;

public class SkeletonMover : EnemyMover
{
    private Vector2 _desiredPosition;
    
    protected override void Move()
    {
        if (ComputeTargetDistance() < enemyData.AttackRange)
        {
            return;
        }
        
        SetClosestPlayerAsTarget();
        ComputeDesiredPosition();

        Vector2 dir = (_desiredPosition - rb.position).normalized;
        rb.MovePosition(rb.position + dir * (speed * Time.fixedDeltaTime));
    }

    private void ComputeDesiredPosition()
    {
        _desiredPosition = target.position;
        _desiredPosition.y = rb.position.y;
    }

    private float ComputeTargetDistance()
    {
        return Vector2.Distance(rb.position, target.position);
    }
    
    private void SetClosestPlayerAsTarget()
    {
        float distance = float.MaxValue;

        foreach (var p in players.Value)
        {
            float d = Vector3.Distance(p.transform.position, transform.position);
            if (d < distance)
            {
                distance = d;
                target = p.transform;
            }
        }
    }
}

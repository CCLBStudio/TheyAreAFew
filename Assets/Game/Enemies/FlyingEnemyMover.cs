using UnityEngine;

public class FlyingEnemyMover : EnemyMover
{
    [SerializeField] private float desiredHeight = 10f;
    [SerializeField] private float minDeltaX, maxDeltaX, minDeltaY, maxDeltaY;
    [SerializeField] private float maxDistanceSpeedMultiplier = 3f;

    private Vector2 _desiredPosition;
    private float _deltaPosX;
    private float _deltaPosY;
    
    protected override void Move()
    {
        SetClosestPlayerAsTarget();
        ComputeDesiredPosition();

        Vector2 dir = (_desiredPosition - rb.position).normalized;
        float dist = Mathf.Min(Vector2.Distance(rb.position, _desiredPosition), maxDistanceSpeedMultiplier);
        rb.MovePosition(rb.position + dir * (speed * dist * Time.fixedDeltaTime));
    }

    private void ComputeDesiredPosition()
    {
        _desiredPosition = target.position;
        _desiredPosition.x += _deltaPosX;
        _desiredPosition.y = desiredHeight + _deltaPosY;
    }

    public override void OnEnemyRequested()
    {
        _deltaPosY = Random.Range(minDeltaY, maxDeltaY);
        _deltaPosX = Random.Range(-minDeltaX, minDeltaX);
        CanMove = true;
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

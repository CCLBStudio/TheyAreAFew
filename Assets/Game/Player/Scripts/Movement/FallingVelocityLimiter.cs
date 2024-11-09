using UnityEngine;

public class FallingVelocityLimiter : MonoBehaviour
{
    [SerializeField][Min(0f)] private float maxAbsoluteFallingVelocity = 8f;
    [SerializeField] private float maximumForceMultiplicator = 10f;
    [SerializeField] private Rigidbody2D rb;
    
    [SerializeField] private Vector2 currentVelocity;

    private void FixedUpdate()
    {
        //rb.linearVelocityY = Mathf.Max(rb.linearVelocityY, -maxAbsoluteFallingVelocity);
        currentVelocity = rb.linearVelocity;

        if (rb.linearVelocityY > 0f)
        {
            return;
        }
        

        float absVel = Mathf.Clamp(-maxAbsoluteFallingVelocity - rb.linearVelocityY, 0f, maximumForceMultiplicator);
        //float absVel = Mathf.Max(-maxAbsoluteFallingVelocity - rb.linearVelocityY, 0f);
        rb.AddForceY(absVel * -Physics2D.gravity.y * rb.gravityScale * rb.mass);
    }
}

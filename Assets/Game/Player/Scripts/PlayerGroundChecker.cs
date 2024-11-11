using UnityEngine;
using UnityEngine.Events;

public class PlayerGroundChecker : MonoBehaviour
{
    public bool Grounded => _grounded;
    
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform feetReference;
    [SerializeField][Range(0.01f, 1f)] private float feetHeightTolerance = .1f;
    [SerializeField][Range(0.01f, 1f)] private float velocityTolerance = .1f;

    [SerializeField] private UnityEvent onGrounded;

    private bool _collideWithGround;
    private bool _grounded;
    private Vector3 _groundPos;

    private void FixedUpdate()
    {
        if (_grounded || !_collideWithGround)
        {
            return;
        }
        
        _grounded = IsGrounded();
        if (_grounded)
        {
            onGrounded?.Invoke();
        }
    }

    private bool IsGrounded()
    {
        bool velocity = Mathf.Abs(rb.linearVelocity.y) < velocityTolerance;
        bool pos = feetReference.position.y >= _groundPos.y - feetHeightTolerance;
        return _collideWithGround && velocity && pos;
    }

    private bool IsLayerGround(int layer)
    {
        return (groundLayers.value & (1 << layer)) != 0;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (IsLayerGround(other.gameObject.layer) && other.gameObject.TryGetComponent(out Ground ground))
        {
            _collideWithGround = true;
            _groundPos = ground.GetGroundPosition();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (IsLayerGround(other.gameObject.layer))
        {
            _collideWithGround = false;
            _grounded = false;
        }
    }
}

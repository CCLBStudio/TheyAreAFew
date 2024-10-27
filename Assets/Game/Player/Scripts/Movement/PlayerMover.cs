using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public bool IsMoving { get; private set; }
    
    [SerializeField] private InputReader inputReader;
    [SerializeField] private float moveSpeed;
    [SerializeField] private PlayerJumper jumper;
    [SerializeField] private Rigidbody2D rb;

    private Vector2 _currentDirection;

    private void Start()
    {
        inputReader.MoveEvent += OnMoveInput;
    }

    private void FixedUpdate()
    {
        if (!IsMoving || jumper.IsChargingJump)
        {
            return;
        }
        
        Move();
    }

    private void OnMoveInput(Vector2 dir)
    {
        IsMoving = dir != Vector2.zero;
        _currentDirection = dir;
    }

    private void Move()
    {
        rb.AddForceX(_currentDirection.x * moveSpeed);
    }
}

using UnityEngine;

public class PlayerMover : MonoBehaviour, IPlayerBehaviour
{
    public bool IsMoving { get; private set; }
    public PlayerFacade Facade { get; set; }
    
    [SerializeField] private InputReader inputReader;
    [SerializeField] private float moveSpeed;
    [SerializeField] private PlayerJumper jumper;
    [SerializeField] private Rigidbody2D rb;

    private Vector2 _currentDirection;
    private bool _propulsorInRange;
    private bool _chargingPropulsion;

    private void Start()
    {
        inputReader.MoveEvent += OnMoveInput;
        inputReader.PropulsionBeginEvent += OnPropulsionInputPressed;
        inputReader.PropulsionReleaseEvent += OnPropulsionInputReleased;
    }

    private void FixedUpdate()
    {
        if (!IsMoving || jumper.IsChargingJump || _chargingPropulsion)
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
    
    #region Player Behaviour Methods
    
    public void OnEnterPropulsor(Propulsor propulsor)
    {
        _propulsorInRange = true;
    }

    public void OnExitPropulsor(Propulsor propulsor)
    {
        _propulsorInRange = false;
    }

    public void OnPropulsionInputPressed()
    {
        _chargingPropulsion = _propulsorInRange;
    }

    public void OnPropulsionInputReleased()
    {
        _chargingPropulsion = false;
    }

    #endregion
}

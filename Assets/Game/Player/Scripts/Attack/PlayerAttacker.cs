using UnityEngine;

public class PlayerAttacker : MonoBehaviour, IPlayerBehaviour
{
    public Transform WeaponHolder => weaponHolder;
    public Rigidbody2D PlayerRb => playerRb;
    public PlayerJumper Jumper { get; private set; }
    public PlayerFacade Facade { get; set; }

    [SerializeField] private InputReader inputReader;
    [SerializeField] private ScriptableWeapon startWeapon;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Rigidbody2D playerRb;
    
    private bool _isShooting;
    private float _shootingTimer;
    private Vector2 _shootingDirection;
    private RuntimeWeapon _currentWeapon;

    #region Unity Events

    void Start()
    {
        inputReader.AimEvent += OnAim;
        _currentWeapon = startWeapon.Equip(this);
        Jumper = GetComponent<PlayerJumper>();
    }

    private void Update()
    {
        weaponPivot.rotation = Quaternion.FromToRotation(Vector3.right, _shootingDirection);
        
        if(_shootingTimer > 0f)
        {
            _shootingTimer -= Time.deltaTime;
            return;
        }

        if(!_isShooting)
        {
            return;
        }
        
        _currentWeapon.Shoot(_shootingDirection);
        _shootingTimer = 1f / _currentWeapon.AttackSpeed;
    }

    #endregion
    
    private void OnAim(Vector2 direction)
    {
        _isShooting = direction != Vector2.zero;
        _shootingDirection = direction.normalized;
    }

    #region Player Behaviour Methods

    public void OnEnterPropulsor(Propulsor propulsor)
    {
    }

    public void OnExitPropulsor(Propulsor propulsor)
    {
    }

    public void OnPropulsionInputPressed()
    {
    }

    public void OnPropulsionInputReleased()
    {
    }

    #endregion
}

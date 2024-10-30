using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private ScriptableWeapon startWeapon;
    [SerializeField] private Transform weaponPivot;
    
    private bool _isShooting;
    private float _shootingTimer;
    private Vector2 _shootingDirection;
    private RuntimeWeapon _currentWeapon;

    #region Unity Events

    void Start()
    {
        inputReader.AimEvent += OnAim;
        _currentWeapon = startWeapon.Equip(this);
    }

    private void Update()
    {
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
    }

    #endregion
    
    private void OnAim(Vector2 direction)
    {
        _isShooting = direction != Vector2.zero;
        _shootingDirection = direction.normalized;
    }
}

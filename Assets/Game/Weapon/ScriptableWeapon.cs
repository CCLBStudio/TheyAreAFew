using System;
using CCLBStudio.ScriptablePooling;
using UnityEngine;

[CreateAssetMenu(menuName = "They Are Many/Weapon/Wepon Scriptable", fileName = "NewWeapon")]
public class ScriptableWeapon : ScriptableObject
{
    public float Damages => damages;
    public float AttackSpeed => attackSpeed;
    public float BulletSpeed => bulletSpeed;
    public float Dispersion => dispersion;
    public RuntimeWeapon WeaponPrefab => weaponPrefab;
    public ScriptablePool BulletPool => bulletPool;
    public GameObject Casing => casing;
    public float CasingEjectionForce => casingEjectionForce;
    public float CasingDispersion => casingDispersion;

    [Tooltip("The base damages of the weapon.")]
    [SerializeField] private float damages;
    [Tooltip("Number of attacks per second.")]
    [SerializeField] private float attackSpeed;
    [SerializeField] private float bulletSpeed;
    [Tooltip("The maximum dispersion for each bullet. Dispersion is a random angle between in range [-dispersion : dispersion] added to the shooting direction.")]
    [SerializeField] private float dispersion;
    [SerializeField] private RuntimeWeapon weaponPrefab;
    [SerializeField] private GameObject casing;
    [SerializeField] private float casingEjectionForce = 20f;
    [SerializeField] private float casingDispersion = 5f;
    [SerializeField] private ScriptablePool bulletPool;


    public RuntimeWeapon Equip(PlayerAttack attacker)
    {
        var weapon = Instantiate(weaponPrefab.gameObject, attacker.WeaponHolder).GetComponent<RuntimeWeapon>();
        weapon.Initialize(attacker);

        return weapon;
    }
}

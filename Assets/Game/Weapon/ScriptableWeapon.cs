using CCLBStudio.ScriptablePooling;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "They Are Many/Weapon/Weapon Scriptable", fileName = "NewWeapon")]
public class ScriptableWeapon : ScriptableObject
{
    public float Damages => damages;
    public float AttackSpeed => attackSpeed;
    public float BulletSpeed => bulletSpeed;
    public float Dispersion => dispersion;
    public RuntimeWeapon WeaponPrefab => weaponPrefab;
    public ScriptablePool BulletPool => bulletPool;
    public ScriptablePool CasingPool => casingPool;
    public ScriptablePool MuzzlePool => muzzlePool;
    public ScriptablePool GroundImpactPool => groundImpactPool;
    public float CasingEjectionForce => casingEjectionForce;
    public float CasingDispersion => casingDispersion;
    public float PlayerKnockbackForce => playerKnockbackForce;
    public float PlayerInAirKnockbackForceXMultiplier => playerInAirKnockbackForceXMultiplier;
    public float BulletLifetime => bulletLifetime;

    [Header("Weapon Visuals")]
    [Tooltip("Weapon model")]
    [SerializeField] private RuntimeWeapon weaponPrefab;
    [SerializeField] private ScriptablePool bulletPool;
    [SerializeField] private ScriptablePool casingPool;
    [SerializeField] private ScriptablePool muzzlePool;
    [SerializeField] private ScriptablePool groundImpactPool;
    
    [Header("Weapon Stats")]
    [Tooltip("The base damages of the weapon.")]
    [SerializeField] private float damages;
    [Tooltip("Number of attacks per second.")]
    [SerializeField] private float attackSpeed;
    [FormerlySerializedAs("playerGroundedKnockbackForce")]
    [Tooltip("The knock back force added to the player on each shoot.")]
    [SerializeField] private float playerKnockbackForce = 1f;
    [Tooltip("When applying knockback in air, multiply the X axis of the knockback direction by this value.")]
    [SerializeField] private float playerInAirKnockbackForceXMultiplier = 5f;
    
    [Header("Bullet Stats")]
    [Tooltip("The bullet travelling speed.")]
    [SerializeField] private float bulletSpeed;
    [Tooltip("The maximum time the bullet will live.")]
    [SerializeField] private float bulletLifetime = 5f;
    [Tooltip("The maximum dispersion for each bullet. Dispersion is a random angle between in range [-dispersion : dispersion] added to the shooting direction.")]
    [SerializeField] private float dispersion;
    [Tooltip("The force at which the casings are ejected.")]
    [SerializeField] private float casingEjectionForce = 5f;
    [Tooltip("The maximum dispersion for each casing. Dispersion is a random angle between in range [-casingDispersion : casingDispersion] added to the ejection direction.")]
    [SerializeField] private float casingDispersion = 20f;


    public RuntimeWeapon Equip(PlayerAttacker attacker)
    {
        var weapon = Instantiate(weaponPrefab.gameObject, attacker.WeaponHolder).GetComponent<RuntimeWeapon>();
        bulletPool.Initialize();
        casingPool.Initialize();
        muzzlePool.Initialize();
        groundImpactPool.Initialize();

        weapon.Initialize(attacker);

        return weapon;
    }
}

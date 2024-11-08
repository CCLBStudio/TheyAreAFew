using CCLBStudio.ScriptablePooling;
using PrimeTween;
using UnityEngine;

[CreateAssetMenu(menuName = "They Are Many/Abilities/Scripable Rocket", fileName = "NewScriptableRocket")]
public class ScriptableRocketAbility : ScriptableAbility
{
    public float TravelSpeed => travelSpeed;
    public float AccelerationTime => accelerationTime;
    public Ease AccelerationEase => accelerationEase;
    public float Lifetime => lifetime;
    public ScriptablePool ExplosionPool => explosionPool;
    public float ExplosionRange => explosionRange;

    [SerializeField] [Min(.1f)] private float travelSpeed;
    [SerializeField] [Min(.1f)] private float accelerationTime = .5f;
    [SerializeField] private float lifetime;
    [SerializeField] private Ease accelerationEase = Ease.InExpo;
    [SerializeField] private ScriptablePool explosionPool;
    [SerializeField] private float explosionRange = 15f;

    public override RuntimeAbility Equip(PlayerAbilities playerAbilities)
    {
        explosionPool.Initialize();
        return base.Equip(playerAbilities);
    }
}

using PrimeTween;
using UnityEngine;

[CreateAssetMenu(menuName = "They Are Many/Abilities/Scripable Rocket", fileName = "NewScriptableRocket")]
public class ScriptableRocketAbility : ScriptableAbility
{
    public float TravelSpeed => travelSpeed;
    public float AccelerationTime => accelerationTime;
    public Ease AccelerationEase => accelerationEase;
    public float Lifetime => lifetime;

    [SerializeField] [Min(.1f)] private float travelSpeed;
    [SerializeField] [Min(.1f)] private float accelerationTime = .5f;
    [SerializeField] private float lifetime;
    [SerializeField] private Ease accelerationEase = Ease.InExpo;
}

using UnityEngine;

public abstract class ScriptableAbility : ScriptableObject
{
    public float Cooldown => cooldown;
    public float Strength => strength;
    public RuntimeAbility AbilityPrefab => abilityPrefab;

    [Header("Stats")]
    [SerializeField][Min(0f)] private float cooldown;
    [SerializeField][Min(0f)] private float strength;

    [Header("Visuals")]
    [SerializeField] private RuntimeAbility abilityPrefab;

    public RuntimeAbility Equip(PlayerAbilities playerAbilities)
    {
        var ability = Instantiate(abilityPrefab.gameObject, playerAbilities.AbilityHolder).GetComponent<RuntimeAbility>();
        ability.Initialize(playerAbilities);
        return ability;
    }
}

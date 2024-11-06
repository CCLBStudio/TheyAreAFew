using UnityEngine;

public abstract class RuntimeAbility : MonoBehaviour
{
    public abstract void Initialize(PlayerAbilities playerAbilities);
    public abstract void ApplyEffect();
    public abstract void OnInputPressed();
    public abstract void OnInputReleased();
}

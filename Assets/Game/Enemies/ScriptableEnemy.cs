using UnityEngine;

[CreateAssetMenu(menuName = "They Are Many/Enemy/Enemy Scriptable", fileName = "NewEneyScriptable")]
public class ScriptableEnemy : ScriptableObject
{
    public float MaxHealth => maxHealth;

    [SerializeField] private float maxHealth = 100f;
}

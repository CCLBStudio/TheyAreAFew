using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private ScriptableWeapon weapon;

    void Start()
    {
        weapon.Equip(transform);
    }
}

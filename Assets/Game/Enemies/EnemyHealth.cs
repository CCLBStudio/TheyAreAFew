using UnityEngine;

public class EnemyHealth : MonoBehaviour, IEnemyBehaviour
{
    public EnemyFacade Facade { get; set; }

    [SerializeField] private ScriptableEnemy enemyData;

    private float _currentHealth;
    
    public void OnEnemyCreated()
    {
        
    }

    public void OnEnemyRequested()
    {
        _currentHealth = enemyData.MaxHealth;
    }

    public void OnEnemyReleased()
    {
    }

    public void OnFixedUpdated()
    {
    }

    public void ApplyDamagesFromDealer(IDamageDealer damageDealer)
    {
        if (_currentHealth <= 0f)
        {
            return;
        }
        
        _currentHealth -= damageDealer.GetDamages();
        if (_currentHealth <= 0f)
        {
            Facade.ReleaseSelf();
        }
    }
}

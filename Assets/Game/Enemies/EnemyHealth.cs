using UnityEngine;

public class EnemyHealth : MonoBehaviour, IEnemyBehaviour, IDamageable
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

    public void GetHit(IDamageDealer damageDealer)
    {
        _currentHealth -= damageDealer.GetDamages();
        if (_currentHealth <= 0f)
        {
            Facade.ReleaseSelf();
        }
    }
}

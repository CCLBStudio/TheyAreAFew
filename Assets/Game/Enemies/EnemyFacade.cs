using System;
using CCLBStudio.ScriptablePooling;
using UnityEngine;

public class EnemyFacade : MonoBehaviour, IScriptablePooledObject
{
    public ScriptablePool Pool { get; set; }
    public ScriptableEnemy EnemyData => enemyData;

    private IEnemyBehaviour[] _behaviours;
    
    [SerializeField] protected ScriptableEnemy enemyData;
    [SerializeField] private EnemyStateMachine stateMachine;

    #region Unity Events

    private void FixedUpdate()
    {
        foreach (var b in _behaviours)
        {
            b.OnFixedUpdated();
        }
    }

    #endregion

    #region Behaviour Methods

    public void ReleaseSelf()
    {
        Pool.ReleaseObject(this);
    }

    public void OnObjectCreated()
    {
        _behaviours = GetComponentsInChildren<IEnemyBehaviour>();

        foreach (var b in _behaviours)
        {
            b.Facade = this;
            b.OnEnemyCreated();

            if (b is IEnemyState state)
            {
                stateMachine.InitState(state);
            }
        }
    }

    public void OnObjectRequested()
    {
        foreach (var b in _behaviours)
        {
            b.OnEnemyRequested();
        }
    }

    public void OnObjectReleased()
    {
        foreach (var b in _behaviours)
        {
            b.OnEnemyReleased();
        }
    }

    #endregion
}

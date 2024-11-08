using System;
using CCLBStudio.ScriptablePooling;
using UnityEngine;

public class EnemyFacade : MonoBehaviour, IScriptablePooledObject
{
    public ScriptablePool Pool { get; set; }

    private IEnemyBehaviour[] _behaviours;

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

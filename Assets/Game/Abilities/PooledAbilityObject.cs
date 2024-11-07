using CCLBStudio.ScriptablePooling;
using UnityEngine;

public abstract class PooledAbilityObject<T> : MonoBehaviour, IScriptablePooledObject where T : ScriptableAbility
{
    public ScriptablePool Pool { get; set; }
    
    [SerializeField] protected T scriptableAbility;

    public abstract void Initialize();

    public virtual void SetPositionAndRotation(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);
    }
    
    public virtual void OnObjectCreated()
    {
    }

    public virtual void OnObjectRequested()
    {
    }

    public virtual void OnObjectReleased()
    {
    }
}

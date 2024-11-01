using CCLBStudio.ScriptablePooling;
using UnityEngine;

public class PooledMuzzle : MonoBehaviour, IScriptablePooledObject
{
    public ScriptablePool Pool { get; set; }
    
    [SerializeField] private float displayTime = .2f;

    private float _timer;

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            Pool.ReleaseObject(this);
        }
    }

    public void OnObjectCreated()
    {
    }

    public void OnObjectRequested()
    {
        _timer = displayTime;
    }

    public void OnObjectReleased()
    {
    }
}

using CCLBStudio.ScriptablePooling;
using UnityEngine;

public class PooledParticleSystem : MonoBehaviour, IScriptablePooledObject
{
    public ScriptablePool Pool { get; set; }

    private ParticleSystem _system;
    private bool _checkForRelease;

    private void Update()
    {
        if (!_checkForRelease)
        {
            return;
        }

        if (!_system.IsAlive())
        {
            Pool.ReleaseObject(this);
        }
    }

    public void Play()
    {
        if (!_system)
        {
            Pool.ReleaseObject(this);
            return;
        }

        _system.Play(true);
        _checkForRelease = true;
    }
    
    public void OnObjectCreated()
    {
        _system = GetComponent<ParticleSystem>();
        if (!_system)
        {
            Debug.LogError("No particle system on objet !");
        }
    }

    public void OnObjectRequested()
    {
    }

    public void OnObjectReleased()
    {
        _checkForRelease = false;
    }
}

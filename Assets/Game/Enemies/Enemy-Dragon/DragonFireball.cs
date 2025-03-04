using CCLBStudio.ScriptablePooling;
using ReaaliStudio.Systems.ScriptableValue;
using UnityEngine;

public class DragonFireball : MonoBehaviour, IScriptablePooledObject
{
    public ScriptablePool Pool { get; set; }
    
    [SerializeField] protected PlayerFacadeListValue players;
    [SerializeField] private float speed = 15f;
    [SerializeField] private float rotationSpeed = 40f;
    [SerializeField] private float lifetime = 3f;

    private Transform _target;
    private float _lifetime;

    private void Update()
    {
        if (!_target)
        {
            return;
        }

        _lifetime -= Time.deltaTime;
        if (_lifetime <= 0f)
        {
            Pool.ReleaseObject(this);
            return;
        }

        var self = transform;
        Vector3 dir = _target.position - self.position;
        //Quaternion lookRot = Quaternion.LookRotation(dir);
        // Quaternion lookRot = Quaternion.LookRotation(Vector3.forward, dir);
        // lookRot *= Quaternion.Euler(0, 90, 0);
        
        Quaternion lookRot = Quaternion.FromToRotation(Vector3.right, dir.normalized);

        self.rotation = Quaternion.Slerp(self.rotation, lookRot, rotationSpeed * Time.deltaTime);
        self.position += self.right * (speed * Time.deltaTime);
    }

    private void SelectClosestTarget()
    {
        float distance = float.MaxValue;

        foreach (var p in players.Value)
        {
            float d = Vector3.Distance(p.transform.position, transform.position);
            if (d < distance)
            {
                distance = d;
                _target = p.transform;
            }
        }
    }

    public void OnObjectCreated()
    {
    }

    public void OnObjectRequested()
    {
        _lifetime = lifetime;
        SelectClosestTarget();
    }

    public void OnObjectReleased()
    {
        _target = null;
    }
}

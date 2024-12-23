using UnityEngine;

public class VfxDestroyer : MonoBehaviour
{
    private ParticleSystem _system;
    void Start()
    {
        _system = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (_system.IsAlive())
        {
            return;
        }
        
        Destroy(gameObject);
    }
}

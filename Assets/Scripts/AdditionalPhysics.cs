using UnityEngine;

public class AdditionalPhysics : MonoBehaviour
{
    public bool grounded;
    public float gravityScale = 1f;

    private Rigidbody _rb;
    
    void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();

        if (!_rb)
        {
            Debug.LogError("No rigidbody attached to this gameobject !");
            Destroy(this);
        }
    }

    void FixedUpdate()
    {
        _rb.AddForce(Physics.gravity * ((gravityScale - 1f) * _rb.mass));
    }
}

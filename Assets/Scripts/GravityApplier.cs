using System;
using UnityEngine;

public class GravityApplier : MonoBehaviour
{
    public bool clearVelocityOnGravityScaleChange = true;
    public float gravityScale = 1f;

    private Rigidbody _rb;
    private float _previousGravityScale = 1f;
    
    void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();

        if (!_rb)
        {
            Debug.LogError("No rigidbody attached to this gameobject !");
            Destroy(this);
            return;
        }

        _rb.useGravity = false;
        _previousGravityScale = gravityScale;
    }

    void FixedUpdate()
    {
        if (clearVelocityOnGravityScaleChange && !Mathf.Approximately(_previousGravityScale, gravityScale))
        {
            _previousGravityScale = gravityScale;
            _rb.linearVelocity = Vector3.zero;
        }
        
        Vector3 gravity = Physics.gravity * gravityScale;
        _rb.AddForce(gravity, ForceMode.Force);
    }
}

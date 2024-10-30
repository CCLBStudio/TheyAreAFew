using CCLBStudio.ScriptablePooling;
using UnityEngine;

public class RuntimeBullet : MonoBehaviour, IScriptablePooledObject
{
    public ScriptablePool Pool { get; set; }
    public Vector2 Direction { get; set; }

    [SerializeField] private ScriptableWeapon weapon;
    [SerializeField] private Rigidbody2D rb;

    private bool _isMoving;

    void FixedUpdate()
    {
        if(!_isMoving)
        {
            return;
        }

        rb.MovePosition(rb.position + Direction * (weapon.BulletSpeed * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Pool.ReleaseObject(this);
    }

    public void OnObjectCreated()
    {
        _isMoving = false;
    }

    public void OnObjectReleased()
    {
        _isMoving = false;
    }

    public void OnObjectRequested()
    {
        _isMoving = true;
    }
}

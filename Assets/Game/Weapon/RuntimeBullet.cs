using CCLBStudio.ScriptablePooling;
using UnityEngine;

public class RuntimeBullet : MonoBehaviour, IScriptablePooledObject
{
    public ScriptablePool Pool { get; set; }
    public Vector2 Direction { get; set; }

    [SerializeField] private Rigidbody2D rb;

    private bool _isMoving;
    private bool _isInit;
    private float _currentLifetime;
    private ScriptableWeapon _currentWeapon;
    private readonly ContactPoint2D[] _contacts = new ContactPoint2D[1];

    void FixedUpdate()
    {
        if(!_isMoving || !_isInit)
        {
            return;
        }

        _currentLifetime -= Time.fixedDeltaTime;
        if (_currentLifetime <= 0f)
        {
            Pool.ReleaseObject(this);
            return;
        }

        rb.MovePosition(rb.position + Direction * (_currentWeapon.BulletSpeed * Time.fixedDeltaTime));
    }

    public void Initialize(ScriptableWeapon weapon)
    {
        _currentWeapon = weapon;
        _currentLifetime = weapon.BulletLifetime;
        _isInit = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var effect = _currentWeapon.GroundImpactPool.RequestObjectAs<PooledParticleSystem>();
        effect.transform.position = other.ClosestPoint(transform.position);
        effect.Play();
        
        Pool.ReleaseObject(this);
    }

    public void OnObjectCreated()
    {
        _isMoving = false;
        _isInit = false;
    }

    public void OnObjectReleased()
    {
        _isMoving = false;
        _isInit = false;
    }

    public void OnObjectRequested()
    {
        _isMoving = true;
    }
}

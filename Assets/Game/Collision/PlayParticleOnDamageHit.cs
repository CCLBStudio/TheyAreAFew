using CCLBStudio.ScriptablePooling;
using UnityEngine;

public class PlayParticleOnDamageHit : MonoBehaviour, IDamageInteractor
{
    [SerializeField] private ScriptablePool effectPool;
    [SerializeField] private bool orientTowardsBullet = true;

    private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        if (!_collider)
        {
            Debug.LogError($"No collider 2D on bullet interactor {name} !");
            Destroy(this);
        }
    }

    public void GetHit(IDamageDealer damageOrigin)
    {
        if (damageOrigin is not RuntimeBullet b)
        {
            return;
        }
        
        var effect = effectPool.RequestObjectAs<PooledParticleSystem>();
        Vector3 bulletPos = b.transform.position;
        effect.transform.position = _collider.ClosestPoint(bulletPos);

        if (orientTowardsBullet)
        {
            Vector3 direction = bulletPos - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            effect.transform.rotation = targetRotation;
        }
        
        effect.Play();
    }
}

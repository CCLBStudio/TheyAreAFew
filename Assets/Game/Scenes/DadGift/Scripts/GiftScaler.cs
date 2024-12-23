using UnityEngine;

public class GiftScaler : MonoBehaviour
{
    public float targetScale = 2f;
    
    private float _initialHealth;
    private Vector3 _initialScale;
    private GiftHealth _health;
    private float _diffScale;
    
    private void Start()
    {
        _health = GetComponent<GiftHealth>();
        _initialHealth = _health.health;
        _initialScale = transform.localScale;
        _diffScale = targetScale - _initialScale.x;
    }

    public void OnHit()
    {
        float healthRatio = 1f - _health.health / _initialHealth;
        transform.localScale = _initialScale + Vector3.one * (_diffScale * healthRatio);
    }
}

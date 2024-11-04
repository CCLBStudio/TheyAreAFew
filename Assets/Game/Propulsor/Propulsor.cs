using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class Propulsor : MonoBehaviour
{
    [SerializeField] private Transform visual;

    private readonly List<IPropulsable> _inRange = new();
    private Tween _scaleTween;
    private float _baseScale;

    private void Start()
    {
        _baseScale = visual.localScale.x;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.TryGetComponent(out IPropulsable propulsable))
        {
            return;
        }
        
        propulsable.EnterPropulsorRange(this);

        if (_inRange.Count <= 0)
        {
            if (_scaleTween.isAlive)
            {
                _scaleTween.Stop();
            }
            
            _scaleTween = Tween.Scale(visual, _baseScale * 2f, .25f, Ease.InOutSine);
        }

        _inRange.Add(propulsable);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.TryGetComponent(out IPropulsable propulsable))
        {
            return;
        }
        
        propulsable.ExitPropulsorRange(this);
        _inRange.Remove(propulsable);
            
        if (_inRange.Count <= 0)
        {
            if (_scaleTween.isAlive)
            {
                _scaleTween.Stop();
            }
                
            _scaleTween = Tween.Scale(visual, _baseScale, .5f, Ease.InOutSine);
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

public class DamageInteractor : MonoBehaviour, IDamageInteractor
{
    public UnityEvent<IDamageDealer> hitEvent;
    public void GetHit(IDamageDealer damageOrigin)
    {
        hitEvent?.Invoke(damageOrigin);
    }
}

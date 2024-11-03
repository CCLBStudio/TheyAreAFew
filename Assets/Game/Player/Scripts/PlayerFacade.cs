using UnityEngine;

public class PlayerFacade : MonoBehaviour
{
    private IPlayerBehaviour[] _behaviours;
    
    private void Awake()
    {
        PlayerManager.Players.Add(this);
        _behaviours = GetComponentsInChildren<IPlayerBehaviour>();

        foreach (var b in _behaviours)
        {
            b.Facade = this;
        }
    }

    public void NotifyPropulsorEnter(Propulsor propulsor)
    {
        foreach (var b in _behaviours)
        {
            b.OnEnterPropulsor(propulsor);
        }
    }
    
    public void NotifyPropulsorExit(Propulsor propulsor)
    {
        foreach (var b in _behaviours)
        {
            b.OnExitPropulsor(propulsor);
        }
    }
    
    public void NotifyPropulsorInputPressed()
    {
        foreach (var b in _behaviours)
        {
            b.OnPropulsionInputPressed();
        }
    }
    
    public void NotifyPropulsorInputReleased()
    {
        foreach (var b in _behaviours)
        {
            b.OnPropulsionInputReleased();
        }
    }
}

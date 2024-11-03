using UnityEngine;

public class PropulsorEventTrigger : MonoBehaviour, IPropulsable
{
    [SerializeField] private PlayerFacade player;
    
    public void EnterPropulsorRange(Propulsor propulsor)
    {
        player.NotifyPropulsorEnter(propulsor);
    }

    public void ExitPropulsorRange(Propulsor propulsor)
    {
        player.NotifyPropulsorExit(propulsor);
    }
}

using ReaaliStudio.Systems.ScriptableValue;
using UnityEngine;

public class PlayerFacade : MonoBehaviour
{
    [SerializeField] private PlayerFacadeListValue players;

    private IPlayerBehaviour[] _behaviours;
    
    private void Awake()
    {
        players.Add(this);
        _behaviours = GetComponentsInChildren<IPlayerBehaviour>();

        foreach (var b in _behaviours)
        {
            b.Facade = this;
        }
    }
}

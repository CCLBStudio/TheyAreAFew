using UnityEngine;

public abstract class SmartState : MonoBehaviour
{
    [SerializeField] protected string stateId;

    public abstract void Enter();
    public abstract void Tick();
    public abstract void Exit();

    public string GetId() => stateId;
}

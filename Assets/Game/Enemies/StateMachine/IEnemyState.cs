public interface IEnemyState
{
    public EnemyStateMachine StateMachine { get; set; }
    void EnterState();
    void UpdateState();
    void ExitState();
    EnemyStateId GetStateId();
}

public enum EnemyStateId {Idle, Chase, Attack, Die}

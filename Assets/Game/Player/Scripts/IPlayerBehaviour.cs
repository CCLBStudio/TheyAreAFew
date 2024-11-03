public interface IPlayerBehaviour
{
    public PlayerFacade Facade { get; set; }
    public void OnEnterPropulsor(Propulsor propulsor);
    public void OnExitPropulsor(Propulsor propulsor);
    public void OnPropulsionInputPressed();
    public void OnPropulsionInputReleased();

}

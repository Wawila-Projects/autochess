public abstract class GameMechanicBase 
{
    protected GameCoordinator coordinator => GameCoordinator.Game;

    protected GameMechanicBase() 
    {
        coordinator.MechanicsController.Add(this);
    }

    public virtual void Start() {}
    public virtual void PreRound() {}
    public virtual void Round() {}
    public virtual void PostRound() {}
    public virtual void End() {}

}
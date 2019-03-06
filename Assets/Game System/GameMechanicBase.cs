public abstract class GameMechanicBase 
{
    protected GameCoordinator Game;

    protected GameMechanicBase() 
    {
    }
    protected GameMechanicBase(GameCoordinator coordinator) 
    {
        Game = coordinator;
        coordinator.MechanicsController.Add(this);
    }

    public virtual void Start() {}
    public virtual void PreRound() {}
    public virtual void Round() {}
    public virtual void PostRound() {}
    public virtual void End() {}

}
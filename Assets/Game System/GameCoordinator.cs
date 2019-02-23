using System.Collections.Generic;

public class GameCoordinator {
    public static GameCoordinator Game => new GameCoordinator();
    
    public List<Player> Players;
    public Dictionary<Player, Player> Opponents;
    public List<GameMechanicBase> MechanicsController;

    void Start() 
    {
        MechanicsController.ForEach((M) => M.Start());
    }

    void Update() 
    {

    }


    
}
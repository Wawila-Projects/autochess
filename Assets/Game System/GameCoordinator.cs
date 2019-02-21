using System.Collections.Generic;

public class GameCoordinator {
    public static GameCoordinator Game => new GameCoordinator();
    
    public List<Player> Players;
    public Dictionary<Player, Player> Opponents;


    
}
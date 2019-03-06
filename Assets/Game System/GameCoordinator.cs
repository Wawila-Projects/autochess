using System.Collections.Generic;
using UnityEngine;

public class GameCoordinator: MonoBehaviour {
    public List<Player> Players = new List<Player>();
    public Dictionary<Player, Player> Opponents = new Dictionary<Player, Player>();
    public List<GameMechanicBase> MechanicsController;

    void Start() 
    {
        MechanicsController?.ForEach((M) => M.Start());
    }
}
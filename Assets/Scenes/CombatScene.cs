using UnityEngine;
using System.Collections.Generic;

public class CombatScene: MonoBehaviour
{
    public Player T1;
    public Player T2;
    public MapCoordinator Map;
    public GameCoordinator Game;

    public List<Character> Team1 = new List<Character>(); 
    public List<Character> Team2 = new List<Character>(); 

    public 

    void Start() 
    {   

        // var origin = Map.Map.Find((T) => T.Hex == new Hex(0,0,0));
        // foreach(var tile in origin.GetTilesInsideRange(3)) 
        // {
        //     tile.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        // }
        // foreach(var tile in origin.GetTilesInsideRange(2)) 
        // {
        //     tile.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        // }
        // foreach(var tile in origin.GetTilesInsideRange(1)) 
        // {
        //     tile.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        // }
        // foreach(var tile in origin.GetTilesInsideRange(0)) 
        // {
        //     tile.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        // }


        Game.Players = new List<Player> {
            T1, T2
        };
        Game.Opponents = new Dictionary<Player, Player>  {
            {T1, T2}, {T2, T1}
        };

        Dictionary<Trait, int> Traits = new Dictionary<Trait, int> {
            {Trait.MovementSpeed, 1}, {Trait.AttackRange, 1}, {Trait.MovementRange, 1} 
        }; 
        Dictionary<AttackModifier, float> AttackModifers = new Dictionary<AttackModifier, float> {
            {AttackModifier.AttackSpeed, 1}
        };

        for (var i = 0; i < T1.transform.childCount; i++)
        {
            var child = T1.transform.GetChild(i).GetComponent<Character>();
            child.AttackModifers = new StatsContainer<AttackModifier, float>(AttackModifers);
            child.Traits = new StatsContainer<Trait, int>(Traits);
            var tile = Map.Map.Find((T) => T.Hex == new Hex(i, 0 , -i));
            child.Location = tile;
            tile.Occupant = child;
            child.transform.position = child.Location.transform.position;
            Team1.Add(child);
            child.init(T1, Game, Map);
        } 
        
        for (var i = 0; i < T2.transform.childCount; i++)
        {
            var child = T2.transform.GetChild(i).GetComponent<Character>();
            child.AttackModifers = new StatsContainer<AttackModifier, float>(AttackModifers);
            child.Traits = new StatsContainer<Trait, int>(Traits);
            var tile =  Map.Map.Find((T) => T.Hex == new Hex(i, 5));
            child.Location = tile;
            tile.Occupant = child;
            child.transform.position = child.Location.transform.position;
            Team2.Add(child);
            child.init(T2, Game, Map);
        } 
    }
} 
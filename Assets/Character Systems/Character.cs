using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

public class Character: MonoBehaviour
{
    public Player Owner;
    public string Name;
    public int Level;
    public int Health;
    public int Mana;
    public Class Class;
    public Race Race;
    public Tile Location; 
    public Tile Destination; 
    public Character Target;
    public StatsContainer<Trait, int> Traits; 
    public StatsContainer<Attribute, int> Attributes;
    public StatsContainer<AttackModifier, float> AttackModifers;
    public bool IsDead;
    public TargetingPriorities TargetPriority;

    [SerializeField]
    private TargetingHandler _Targeter ;
    private GameCoordinator _Game;
    private MapCoordinator _Map;

    public void init(Player owner, GameCoordinator game, MapCoordinator map) {
        Owner = owner;
        Owner.Team.Add(this);
        _Targeter = new TargetingHandler(this, game);
        _Targeter.Priority = TargetPriority;
        _Game = game;
        _Map = map;
    }

    IEnumerator Start() 
    {
            yield return new WaitUntil(() => { return !IsDead && 
                                                    Owner != null &&
                                                    _Map.DoneShowing; });
            StartCoroutine(Act());
    }

     private IEnumerator Act() {
         while(!IsDead) {
            Target = _Targeter.GetTarget();
            if (Target == null) continue;

            Destination = GetPosition(Target);
            if(Destination != null) 
            {
                Move(Destination);
                //print($"{name} Moving to Target {Destination.name}");
                yield return new WaitForSeconds(Traits[Trait.MovementSpeed]);
            }
            print($"{name} Attacking {Target.name}");
            yield return new WaitForSeconds(AttackModifers[AttackModifier.AttackSpeed]);
         }
         Location.Occupant = null;
         Location = null;
    }

    private Tile GetPosition(Character target) 
    {
        var destination = _Targeter.GetDestination(target);
        if (destination == null) return null;
        var path = AStar.FindPath(Location, destination);
        var next = path.NextStep(Location, Traits[Trait.MovementRange]);
        if (next?.IsOccupied == true) {
            next = Location.Neighbors.FindAll((T) => { return !T.IsOccupied; })
                        .OrderBy((T) => { return T.GetDistance(destination); })
                        .FirstOrDefault();
        }
        return next;
    }

    private bool Move(Tile tile) 
    {
        if (tile == null) return false;
        transform.position = tile.transform.position;
        Location.Occupant = null;
        Location = tile;
        Location.Occupant = this;
        return true;
    }

    public void OnDrawGizmos() {
        if (Target is null) return;

        var text = Target.name;
        var style = new GUIStyle() {
            fontSize = 8
        };
        var position = transform.position + new Vector3(0,0,5);
        Handles.Label(position, text, style);
    }
}
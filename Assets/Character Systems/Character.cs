using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

public class Character: MonoBehaviour
{
    public Player Owner;
    public string Name;
    public int Level;
    public float Health;
    public float Mana;
    public Class Class;
    public Race Race;
    public Tile Location; 
    public Tile Destination; 
    public Character Target;
    public StatsContainer<Trait, float> Traits; 
    public StatsContainer<Attribute, int> Attributes;
    public StatsContainer<AttackModifier, float> AttackModifiers;
    public bool IsDead;
    public TargetingPriorities TargetPriority;

    [SerializeField]
    private TargetingHandler _Targeter ;
    private GameCoordinator _Game;
    private MapCoordinator _Map;

    private Attribute _primaryAttribute = Attribute.Strength;

    public void init(Player owner, GameCoordinator game, MapCoordinator map) {
        Owner = owner;
        Owner.Team.Add(this);
        Health = Traits[Trait.Health];
        _Targeter = new TargetingHandler(this, game);
        _Targeter.Priority = TargetPriority;
        _Game = game;
        _Map = map;
    }

    public void Revive(Tile location)
    {
        if (!IsDead) return;
        IsDead = false;
        Location = location;
        Location.Occupant = this;
        transform.position = location.transform.position;
        GetComponent<Renderer>().enabled = true;
        StartCoroutine(Act());
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
            if (Target == null) 
            {
                yield return null;
                continue;
            }
            Destination = GetPosition(Target);
            if(Destination != null) 
            {
                Move(Destination);
                yield return new WaitForSeconds(Traits[Trait.MovementSpeed]);
            }
            Combat(Target);
            yield return new WaitForSeconds(AttackModifiers[AttackModifier.AttackSpeed]);
         }
         Location.Occupant = null;
         Location = null;
         GetComponent<Renderer>().enabled = false;
    }

    private Tile GetPosition(Character target) 
    {
        var destination = _Targeter.GetDestination(target);
        if (destination == null) return null;
        var path = AStar.FindPath(Location, destination);
        var next = path.NextStep(Location, (int)Traits[Trait.MovementRange]);
        if (next?.IsOccupied == true) {
            next = Location.Neighbors.FindAll((T) => { return !T.IsOccupied; })
                        .OrderBy((T) => { return T.GetDistance(destination); })
                        .FirstOrDefault();
        }
        return next;
    }

    private void Combat(Character target)
    {
        ResolveDamage(target, false);

        var cleave = AttackModifiers[AttackModifier.Cleave];
        if (cleave <= 0) return ;
        
        var cleavedTiles = target.Location.Neighbors.Intersect(Location.Neighbors)
                .Where((T) => T.IsOccupied && T.Occupant != target && T.Occupant.Owner != Owner);

        foreach (var tile in cleavedTiles)
        {
            if (tile.Occupant == null) continue;
            ResolveDamage(tile.Occupant, true);
        }
    }

    private void ResolveDamage(Character target, bool isCleave) 
    {
        var damage = CalculateDamage(target);

        if (isCleave) 
        {
            damage *= AttackModifiers[AttackModifier.Cleave];
            print($"{name} Attacking {Target.name}| Cleave Damage: {damage}");
        } else 
        {
            print($"{name} Attacking {Target.name}| Damage: {damage}");

        }

        var lifeSteal = damage * AttackModifiers[AttackModifier.Lifesteal];
        var manaBreak = damage * AttackModifiers[AttackModifier.ManaBreak];

        target.Health -= damage;
        target.Mana -= manaBreak;
        Health += lifeSteal;

        if (target.Health <= 0)
        {
            target.Health = 0;
            target.IsDead = true;
        }

        if (Health > Traits[Trait.Health])
            Health = Traits[Trait.Health];

        if (target.Mana < 0)
            target.Mana = 0;
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

    private float CalculateDamage(Character target) 
    {
        var mainDamage = AttackModifiers[AttackModifier.Damage] + Attributes[_primaryAttribute];
        
        var armor = target.AttackModifiers[AttackModifier.Armor];
        var armorMultiplier = 1f - ((0.052f * armor) / (0.9f + 0.048f * Mathf.Abs(armor)));

        return mainDamage - armorMultiplier;
    }

    // public void OnDrawGizmos() {
    //     if (Traits is null) return;

    //     var text = $"{Health}/{Traits[Trait.Health]}";
    //     var style = new GUIStyle() {
    //         fontSize = 8
    //     };
    //     var position = transform.position + new Vector3(0,0,5);
    //     Handles.Label(position, text, style);
    // }
} 
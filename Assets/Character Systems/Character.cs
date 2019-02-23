using UnityEngine;

public abstract class Character: MonoBehaviour
{
    public Player Owner;

    public string Name => Unit.Name;
    public int Level;
    public int Health;
    public int Mana;
    public Class Class => Unit.Class;
    public Race Race => Unit.Race;
    public Tile Location; 
    public Character Target;
    public StatsContainer<Trait, int> Traits; 
    public StatsContainer<Attribute, int> Attributes;
    public StatsContainer<AttackModifier, float> AttackModifers;

    public IUnit Unit;
    public bool IsDead;
    private TargetingHandler _Targeter;

    public void init(Player owner, IUnit unit) {
        Owner = owner;
        Level = unit.Level;
        Unit = unit;
    }
}
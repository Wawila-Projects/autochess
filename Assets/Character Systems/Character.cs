using UnityEngine;

public class Character: MonoBehaviour
{

    public Player Owner;
    public StatsContainer<Trait, int> Traits; 
    public StatsContainer<Attribute, int> Attributes;
    public StatsContainer<AttackModifier, float> AttackModifers;

    private TargetingHandler _Targeter;

    public Tile Location; 
    public Character Target;
    public bool IsDead;
    
}
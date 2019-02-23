using UnityEngine;
public interface IUnit 
{
    string Name { get; }
    int Cost { get; }
    int Level { get; }
    Rarity Rarity { get; }
    Class Class { get; }
    Race Race { get; }
    GameObject Prefab { get; set; }
    bool SetOwner(Player owner);
}
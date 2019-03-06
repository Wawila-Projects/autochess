using System.Collections.Generic;
using System.Linq;
public class UnitStore: GameMechanicBase
{
    public IDictionary<int, List<IUnit>> UnitsForSale;
    private IDictionary<string, Stack<IUnit>> UnitBuckets;

    public UnitStore(): base() {
        UnitsForSale = new Dictionary<int, List<IUnit>>();
        UnitBuckets = new Dictionary<string, Stack<IUnit>>();
    }

    public override void PreRound() {
        SetUnitsForSale();
    } 

    public override void PostRound() {
        ReturnUnitsNotSold();
    }

    public void AddUnitsToBucket(List<IUnit> units) 
    {
        foreach(var unit in units ) 
        {
            for (var i = 0; i < (int)unit.Rarity; i++)
            {
                if (UnitBuckets[unit.Name] == null) 
                {
                    UnitBuckets[unit.Name] = new Stack<IUnit>();
                }
                UnitBuckets[unit.Name].Push(unit);
            }
        }    
    }

    private void SetUnitsForSale() 
    {
        var users = Game.Players
                        .Where((P)=> !P.Defeated)
                        .Select((P) => P.GetHashCode());

        foreach(var user in users) 
        {
            UnitsForSale[user] = GetRandomForSale();
        }
    }
    
    private void ReturnUnitsNotSold() 
    {
        foreach(var bucket in UnitsForSale) 
        {
            foreach (var unit in bucket.Value)
            {
                UnitBuckets[unit.Name].Push(unit);
            }
        }
        UnitsForSale.Clear();
    }

    private List<IUnit> GetRandomForSale() 
    {
        var units = new List<IUnit>();
        var keys = UnitBuckets.Keys;
        for (var i = 0; i < 5; i++)
        {
            var key = keys.RandomElement();
            var bucket = UnitBuckets[key];
            var unit = bucket.Pop(); 
            if (bucket.Count == 0 || unit == null) 
            {
                --i;
                continue;
            }

            units.Add(unit);
        }
        return units;
    }
}
using System.Linq;
using System.Collections.Generic;

public class TargetingHandler 
{
    public TargetingPriorities Priority;
    private Character _parent;
    private List<Character> _opponents => GameCoordinator.Game.Opponents[_parent.Owner]
                                                        .Team.FindAll((O) => !O.IsDead);

    public TargetingHandler(Character parent) {
        _parent = parent;
        Priority = TargetingPriorities.Random;
    }

    public Tile GetDestination(Character target) {
        var range = _parent.Traits[Trait.AttackRange];
        var origin = _parent.Location;
        var targetTile = target.Location;

        if(targetTile.GetDistance(origin) <= range)
        {
            return null;
        }

        var tilesInRange = target.Location.GetTilesInsideRange(range)
                    .FindAll((T) => !T.isObstacle && !T.IsOccupied );

        if (tilesInRange.Count == 0)
        {
            return origin;
        }

        var destination = tilesInRange.OrderBy((T) => T.GetDistance(origin)).First();
        return destination;
    }

    public Character GetTarget() {
        switch (Priority)
        {
            case TargetingPriorities.Random:
                return _opponents.RandomElement();
            case TargetingPriorities.Attacker:
                return TargetAttacker();
            case TargetingPriorities.Closest:
                return TargetClosest();
            case TargetingPriorities.Farthest:
                return TargetFarthest();
            case TargetingPriorities.MaxHealthDynamic:
                return TargetMaxHealth();
            case TargetingPriorities.LeastHealthDynamic:
                return TargetLeastHealth();
            case TargetingPriorities.MaxHealthStatic:
                return TargetMaxHealthStatic();
            case TargetingPriorities.LeastHealthStatic:
                return TargetLeastHealthStatic();
        }
        return null;
    }

    private Character TargetAttacker() => _opponents.Find((C) => C.Target == _parent) ?? _opponents.RandomElement();
    private Character TargetFarthest() => _opponents.OrderBy((O) => O.Location.GetDistance(_parent.Location)).First();
    private Character TargetClosest() =>  _opponents.OrderByDescending((O) => O.Location.GetDistance(_parent.Location)).First();
    private Character TargetLeastHealth() => _opponents.OrderBy((O) => O.Traits[Trait.Health]).First();
    private Character TargetMaxHealth() => _opponents.OrderByDescending((O) => O.Traits[Trait.Health]).First();    
    private Character TargetLeastHealthStatic() => _opponents.OrderBy((O) => O.Traits[Trait.MaxHealth]).First();
    private Character TargetMaxHealthStatic() => _opponents.OrderByDescending((O) => O.Traits[Trait.MaxHealth]).First();    
}
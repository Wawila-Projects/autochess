using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class TargetingHandler 
{
    public TargetingPriorities Priority;
    public Character Target;
    private Character _parent;
    private List<Character> _opponents;
    private GameCoordinator _game;
    [UnityEngine.SerializeField]
    private Character _prevTarget;

    public TargetingHandler(Character parent, GameCoordinator game) {
        _parent = parent;
        Priority = TargetingPriorities.Random;
        _game = game;
    }

    public Tile GetDestination(Character target) {
        var range = _parent.Traits[Trait.AttackRange];
        var origin = _parent.Location;
        var targetTile = target.Location;

        if(targetTile.GetDistance(origin) <= range)
        {
            return null;
        }

        if(targetTile == origin)
        {
            return null;
        }

        var tilesInRange = targetTile.GetTilesInsideRange((int)range);
        tilesInRange = tilesInRange.FindAll((T) => !T.isObstacle && !T.IsOccupied);

        if (tilesInRange.Count == 0)
        {
            _prevTarget = Target;
            Target = null;
            UnityEngine.Debug.Log($"{_parent.name} Changing Target");
            return origin;
        }

        var destination = tilesInRange.OrderBy((T) => T.GetDistance(origin)).First();
        return destination;
    }

    public Character GetTarget() {
        if (Target != null && !Target.IsDead)
        {
            return Target;
        }
        
        _opponents = _game.Opponents[_parent.Owner]
                                .Team.FindAll((O) => !O.IsDead && O != _prevTarget);

        switch (Priority)
        {
            case TargetingPriorities.Random:
                Target = _opponents.RandomElement();
                break;
            case TargetingPriorities.Attacker:
                Target =  TargetAttacker();
                break;
            case TargetingPriorities.Closest:
                Target =  TargetClosest();
                break;
            case TargetingPriorities.Farthest:
                Target =  TargetFarthest();
                break;
            case TargetingPriorities.MaxHealthDynamic:
                Target =  TargetMaxHealth();
                break;
            case TargetingPriorities.LeastHealthDynamic:
                Target =  TargetLeastHealth();
                break;
            case TargetingPriorities.MaxHealthStatic:
                Target =  TargetMaxHealthStatic();
                break;
            case TargetingPriorities.LeastHealthStatic:
                Target =  TargetLeastHealthStatic();
                break;
            default:
                Target = null;
                break;
        }
        return Target;
    }

    private Character TargetAttacker() => _opponents.Find((C) => C.Target == _parent) ?? _opponents.RandomElement();
    private Character TargetFarthest() => _opponents.OrderByDescending((O) => O.Location.GetDistance(_parent.Location)).FirstOrDefault();
    private Character TargetClosest() =>  _opponents.OrderBy((O) => O.Location.GetDistance(_parent.Location)).FirstOrDefault();
 
    private Character TargetLeastHealth() => _opponents.OrderBy((O) => O.Health).FirstOrDefault();
    private Character TargetMaxHealth() => _opponents.OrderByDescending((O) => O.Health).FirstOrDefault();    
    private Character TargetLeastHealthStatic() => _opponents.OrderBy((O) => O.Traits[Trait.Health]).FirstOrDefault();
    private Character TargetMaxHealthStatic() => _opponents.OrderByDescending((O) => O.Traits[Trait.Health]).FirstOrDefault();    
}
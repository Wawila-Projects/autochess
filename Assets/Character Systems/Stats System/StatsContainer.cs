using System.Collections.Generic;

public class StatsContainer<TKey, TValue> where TKey: System.Enum {
    protected IDictionary<TKey, TValue> Stats;
    protected IDictionary<TKey, TValue> Buffs;

    public StatsContainer(IDictionary<TKey, TValue> stats, IDictionary<TKey, TValue> buffs = null) 
    {
        Stats = stats;
        if(buffs != null) 
        {
            Buffs = buffs;
            return;
        }

        Buffs = new Dictionary<TKey, TValue>();
        foreach(TKey key in System.Enum.GetValues(typeof(TKey)) ) {
            Stats[key] = default(TValue);
            Buffs[key] = default(TValue);
        }
    }

    public StatsContainer()
    {
        Stats = new Dictionary<TKey, TValue>();
        Buffs = new Dictionary<TKey, TValue>();
        foreach(TKey key in System.Enum.GetValues(typeof(TKey)) ) {
            Stats[key] = default(TValue);
            Buffs[key] = default(TValue);
        }
    } 

    public TValue this[TKey key] => GetFinalStat(key);

    public virtual TValue GetFinalStat(TKey key) {
        return Add(Stats[key], Buffs[key]);
    }
    public virtual TValue GetStat(TKey key) {
        return Stats[key];
    }
    public virtual TValue GetBuff(TKey key) {
        return Buffs[key];
    }

    public virtual void SetStat(TKey key, TValue value) {
        Stats[key] = value;
    }
    public virtual void SetBuff(TKey key, TValue value) {
        Buffs[key] = value;
    }

    public virtual TValue AddStat(TKey key, TValue value) {
        var total = Add(Stats[key], value);
        Stats[key] = total;
        return total;
    }

    public virtual TValue AddBuff(TKey key, TValue value) {
        var total = Add(Buffs[key], value);
        Buffs[key] = total;
        return total;
    }

    public virtual TValue SubtractStat(TKey key, TValue value) {
        var total = Subtract(Stats[key], value);
        Stats[key] = total;
        return total;
    }
    
    public virtual TValue SubtractBuff(TKey key, TValue value) {
        var total = Subtract(Buffs[key], value);
        Buffs[key] = total;
        return total;
    }

    public virtual void ResetBuff(TKey key) {
        Buffs[key] = default(TValue);
    }
    
    protected virtual TValue Add(TValue lhs, TValue rhs) {
        dynamic left = lhs;
        dynamic right = lhs;
        return (TValue)(left+right);
    }

    protected virtual TValue Subtract(TValue lhs, TValue rhs) {
        dynamic left = lhs;
        dynamic right = lhs;
        return (TValue)(left-right);
    }
}
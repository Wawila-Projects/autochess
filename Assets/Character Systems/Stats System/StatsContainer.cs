using System;
using System.Collections.Generic;

public class StatsContainer<TKey, TValue> where TKey: Enum {
    protected IDictionary<TKey, TValue> Stats;
    protected IDictionary<TKey, TValue> Buffs;

    protected bool IsPercentage;

    public StatsContainer(IDictionary<TKey, TValue> stats, IDictionary<TKey, TValue> buffs = null, bool isPercentage = false) 
    {
        Stats = stats;
        IsPercentage = isPercentage;
        if(buffs != null) 
        {
            Buffs = buffs;
            return;
        }

        Buffs = new Dictionary<TKey, TValue>();
        foreach(TKey key in System.Enum.GetValues(typeof(TKey)) ) {
            Buffs[key] = default(TValue);
            if (!Stats.ContainsKey(key)) {
                Stats[key] = default(TValue);
            } 
        }
    }

    public StatsContainer(bool isPercentage = false)
    {
        IsPercentage = isPercentage;
        Stats = new Dictionary<TKey, TValue>();
        Buffs = new Dictionary<TKey, TValue>();
        foreach(TKey key in System.Enum.GetValues(typeof(TKey)) ) {
            Stats[key] = default(TValue);
            Buffs[key] = default(TValue);
        }
    } 

    public TValue this[TKey key] => GetFinalStat(key);

    public virtual TValue GetFinalStat(TKey key) {
        var value = Add(Stats[key], Buffs[key]);
        var percentage = Divide(value, IsPercentage ? 100 : 1);
        return percentage;
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
        if(lhs is int lhsi && rhs is int rhsi)
            return (TValue)(object)(lhsi+rhsi);
        if(lhs is double lhsd && rhs is double rhsd)
            return (TValue)(object)(lhsd+rhsd);
        return lhs;
    }

    protected virtual TValue Subtract(TValue lhs, TValue rhs) {
       if(lhs is int lhsi && rhs is int rhsi)
            return (TValue)(object)(lhsi-rhsi);
        if(lhs is double lhsd && rhs is double rhsd)
            return (TValue)(object)(lhsd-rhsd);
        return lhs;
    }
    protected virtual TValue Divide(TValue lhs, int rhs) {
       if(lhs is int lhsi)
            return (TValue)(object)(lhsi/rhs);
        if(lhs is double lhsd)
            return (TValue)(object)(lhsd/rhs);
        return lhs;
    }
}
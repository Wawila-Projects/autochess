using System;
using System.Linq;
using System.Collections.Generic;

public static class Extensions {
    private static Random random => new Random();
    public static T RandomElement<T>(this ICollection<T> c) {
        if (c.Count == 0) 
        {
            return default(T);
        }
        var index = random.Next(0, c.Count);
        return c.ElementAt(index);
    }

    public static Tile NextStep(this List<Tile> path, Tile current, int amount = 1) 
    {
        var index = path.IndexOf(current);
        if (index == -1) return null;
        var next = index + amount;
        if (path.Count < next) {
            return path.LastOrDefault();
        }
        return  path.ElementAtOrDefault(index+amount);
    }
}
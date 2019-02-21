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
}
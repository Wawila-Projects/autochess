using System;
using System.Collections.Generic;


///<summary>
/// For more info refer to:
/// https://www.redblobgames.com/grids/hexagons/implementation.html#map
///</summary>
public class MapConstructor {
    public static HashSet<Hex> TriangleMap(int size) {
        var map = new HashSet<Hex>();
        for (var q = 0; q < size; q++)
            for (var r = 0; r < size-q; r++)
                map.Add(new Hex(q, r, true));   
        return map;
    }

    public static HashSet<Hex> ReverseTriangleMap(int size) {
        var map = new HashSet<Hex>();

        for (var q = 0; q < size; q++)
            for (var r = size-q; r <= size; r++)
                map.Add(new Hex(q, r, true));   

        return map;
    }

    public static HashSet<Hex> HexagonalMap(int radius) 
    {
        var map = new HashSet<Hex>();

        for (int q = -radius; q <= radius; q++) 
        {
            int r1 = Math.Max(-radius, -q - radius);
            int r2 = Math.Min(radius, -q + radius);
            for (int r = r1; r <= r2; r++)
                map.Add(new Hex(q, r, true));
        }

        return map;
    }

    public static HashSet<Hex> RectangularlMap(int width, int height) {
        var map = new HashSet<Hex>();

        for (int r = 0; r < height; r++) 
        {
            int r_offset = r>>1;
            for (int q = -r_offset; q < width - r_offset; q++) 
                map.Add(new Hex(q, r, true));
        }

        return map;
    }
}
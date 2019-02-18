using UnityEngine;
using System.Collections.Generic;
public enum MapShapes 
{
    Triangle, ReverseTrianlge, Hexagonal, Rectangular
}

public enum MapType
{
    PointyEven, PointyOdd, FlatEven, FlatOdd
}

public class MapCoordinator: MonoBehaviour 
{
    public MapShapes Shape = MapShapes.Rectangular;
    public MapType Type = MapType.PointyEven;

    public List<Tile> Map;
    public int Width;
    public int Height;
    public GameObject Prefab;


    void Update() 
    {
        if(!Input.GetKeyDown(KeyCode.Space)) return;
            
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        Map = new List<Tile>();
        var map = new HashSet<Hex>();

        switch(Shape) {
            case MapShapes.Triangle:
                map = MapConstructor.TriangleMap(Width);
                break;
            case MapShapes.ReverseTrianlge:
                map = MapConstructor.ReverseTriangleMap(Width);
                break;
            case MapShapes.Rectangular:
                map = MapConstructor.RectangularlMap(Width, Height);
                break;
            case MapShapes.Hexagonal:
                map = MapConstructor.HexagonalMap(Width);
                break;
        }

        foreach(var hex in map) {
            var prefab = GameObject.Instantiate(Prefab, hex, Quaternion.identity);
            prefab.name = $"Hex {hex.Q}, {hex.R}, {hex.S}";
            var tile = prefab.GetComponent<Tile>();

            var isEven = Type == MapType.PointyOdd || Type == MapType.FlatOdd || Shape != MapShapes.Rectangular;
            if (Type == MapType.FlatEven || Type == MapType.FlatOdd) {
                tile.InitFlat(hex, isEven);
            } else  {
                tile.InitPointy(hex, isEven);
            }
            Map.Add(tile);
            prefab.transform.SetParent(transform);
        }
    }

}
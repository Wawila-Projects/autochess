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

    public Tile Start;
    public Tile End;

    public List<Tile> Map;
    public int Width;
    public int Height;
    public GameObject Prefab;

    void Awake() {
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

        var obstacles = new List<Hex>() {
            new Hex(1, 0 ,-1), new Hex(1,1,-2), new Hex(0,3,-3), new Hex(1,4,-5),
            new Hex(2,3,-5), new Hex(2,2,-4), new Hex(3,1,-4), new Hex(0,2,-2)
        };

        foreach(var hex in map) {
            var prefab = GameObject.Instantiate(Prefab, hex, Quaternion.identity);
            prefab.name = hex.ToString();
            var tile = prefab.GetComponent<Tile>();

            if (tile == null) continue;

            var isEven = Type == MapType.PointyOdd || Type == MapType.FlatOdd || Shape != MapShapes.Rectangular;
            if (Type == MapType.FlatEven || Type == MapType.FlatOdd) {
                tile.InitFlat(hex, isEven);
            } else  {
                tile.InitPointy(hex, isEven);
            }
            Map.Add(tile);
            prefab.transform.SetParent(transform);

            if(obstacles.Contains(hex)) {
                prefab.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                prefab.transform.localScale  = new Vector3(1,1,0.3f);
                var z = (1-prefab.transform.localScale.z) * 0.125f;
                prefab.transform.position += new Vector3(0,0,z);
                tile.isObstacle = true;
            }
        }

        Map.ForEach((T) => T.GetNeighbors());

        Start = Map.Find(T => T.name == new Hex(0,0,0).ToString());
        End = Map.Find(T => T.name == new Hex(2,4,-6).ToString());

        transform.rotation = Quaternion.Euler(90,0,0);
    }

    void Update() 
    {
        if(!Input.GetKeyDown(KeyCode.Space)) return;

        AStar pf = new AStar();
        var path = pf.FindPath(Start, End);
        foreach(var tile in path) {
            tile.GetComponent<Renderer>()?.material.SetColor("_Color", Color.red);
        }
    }

}
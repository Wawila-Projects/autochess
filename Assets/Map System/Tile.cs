using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Tile: MonoBehaviour
{
    public Hex Hex;
    public List<Tile> Neighbors;
    public bool isObstacle;
    public GameObject Prefab;
    public int WorldX => _coordinates.Column;
    public int WorldY => _coordinates.Row;
    public OffsetCoordinates _coordinates;
    public Layout Layout;

    public void Init(Hex hex) {
        isObstacle =  tag == "Obstacle";
        Hex = hex;
        GetNeighbors();
    }

    public void InitPointy(Hex hex, bool even = true) {
        Hex = hex;
        float x;
        if (even) {
            _coordinates = OffsetCoordinates.EvenPointyOffset(hex);
            x = WorldY % 2 == 0 ? Mathf.Sqrt(3f)*WorldX : 
                                    Mathf.Sqrt(3f)*(WorldX-1)+(Mathf.Sqrt(3f)/2f);
        } else {
            _coordinates = OffsetCoordinates.OddPointyOffset(hex);
            x = WorldY % 2 == 0 ? Mathf.Sqrt(3f)*WorldX : 
                                    Mathf.Sqrt(3f)*(WorldX-1)+(Mathf.Sqrt(3f)/2f);
        }
        var y = WorldX % 2 == 0 ? 3f*WorldY/2f : 3f*WorldY;
        transform.position = new Vector3(x, 3f*WorldY/2f);
    }

    public void InitFlat(Hex hex, bool even = true) {
        Hex = hex;
        //gameObject.transform.rotation = Quaternion.Euler(0,0,30);
        if (even) {
            _coordinates = OffsetCoordinates.EvenFlatOffset(hex);
        } else {
            _coordinates = OffsetCoordinates.OddFlatOffset(hex);
        }
        var x = WorldY % 2 == 0 ? Mathf.Sqrt(3f)*WorldX : Mathf.Sqrt(3f)*(WorldX-1)+(Mathf.Sqrt(3f)/2f);
        var y = WorldX % 2 == 0 ? 3f*WorldY/2f : 3f*WorldY;
        transform.position = new Vector3(x, 3f*WorldY/2f);
    }

    public void GetNeighbors() {
        Neighbors = new List<Tile>();
        foreach(var hex in Hex.Neighbors) {
            var go = GameObject.Find(hex.ToString());
            if (go != null)
                Neighbors.Add(go.GetComponent<Tile>());
        }
    }

    public void OnDrawGizmos() {
            if (Hex is null) return;

            var text = Hex.ToString();
            var style = new GUIStyle() {
                fontSize = 8
            };
            var position = transform.position;
            Handles.Label(position, text, style);
        }
}

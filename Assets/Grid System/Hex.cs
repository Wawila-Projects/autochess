using System;
using System.Collections.Generic;

[System.Serializable]
public class Hex : HexBase<int>
{
    public readonly int Length;
    public readonly List<Hex> Neighbors;
    public readonly List<Hex> DiagonalNeighbors;

    public Hex(int q, int r, int s, bool forTile = false) : base(q, r, s)
    {
        if (!forTile) return;

        Length = GetLength(this);
        Neighbors = GetNeighbors(this);
        DiagonalNeighbors = GetDiagonalNeighbors(this);
    }

    public Hex(int q, int r,bool forTile = false): this(q, r, -q-r, forTile) { }

    public override string ToString() {
        return $"Hex {Q}, {R}, {S}";
    }

    public int GetDistance(Hex other) 
    {
        return GetDistance(this, other);
    }

    public Hex Scale(int k) 
    {
        return this * k;
    }

    public Hex RotateLeft() {
        return new Hex(-S, -Q, -R);
    } 
    public Hex RotateRight() {
        return new Hex(-R, -S, -Q);
    } 

    /// Distance from Center
    public static int GetLength(Hex hex) 
    {
        return (int)((Math.Abs(hex.Q) + Math.Abs(hex.R) + Math.Abs(hex.S))/2);
    }
    
    /// Distance Between Two Hexes
    public static int GetDistance(Hex lhs, Hex rhs) 
    {
        return GetLength(lhs - rhs);
    }

    public static List<Hex> GetNeighbors(Hex hex)
    {
        var directions = new List<Hex> {
            new Hex(1, 0, -1), new Hex(1, -1, 0), new Hex(0, -1, 1),
            new Hex(-1, 0, 1), new Hex(-1, 1, 0), new Hex(0, 1, -1)
        };

        var neighbors = new List<Hex>();
        foreach(var direction in directions) 
        {
            neighbors.Add(hex + direction);
        }
        return neighbors;
    }  

    public static List<Hex> GetDiagonalNeighbors(Hex hex)
    {
        var directions = new List<Hex> {
            new Hex(2, -1, -1), new Hex(1, -2, 1), new Hex(-1, -1, 2), 
            new Hex(-2, 1, 1), new Hex(-1, 2, -1), new Hex(1, 1, -2)
        };

        var neighbors = new List<Hex>();
        foreach(var direction in directions) 
        {
            neighbors.Add(hex + direction);
        }
        return neighbors;
    }  

    public static implicit operator UnityEngine.Vector3 (Hex hex) {
        return new UnityEngine.Vector3(hex.Q, hex.R, hex.S);
    } 

    public override bool Equals(Object other)
    {   
        if (other is Hex hex) { 
            return Equals(hex);
        }
        return false;
    }

    public override int GetHashCode() {
        return Q.GetHashCode() ^ R.GetHashCode() ^ S.GetHashCode();
    }

    public static bool operator ==(Hex lhs, Hex rhs) {
        return lhs.Equals(rhs);
    }

    public static bool operator !=(Hex lhs, Hex rhs) {
        return !lhs.Equals(rhs);
    }
    
    public static Hex operator +(Hex lhs, Hex rhs) {
        return new Hex(lhs.Q + rhs.Q, lhs.R + rhs.R, lhs.S + rhs.S);
    }

    public static Hex operator -(Hex lhs, Hex rhs) {
        return new Hex(lhs.Q - rhs.Q, lhs.R - rhs.R, lhs.S - rhs.S);
    }

    public static Hex operator *(Hex lhs, int k) {
        return new Hex(lhs.Q * k, lhs.R * k, lhs.S * k);
    }
}

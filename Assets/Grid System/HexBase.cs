using System;
using System.Collections.Generic;
public abstract class HexBase<T> : IEquatable<HexBase<T>> where T: IEquatable<T>
{
    public readonly T Q, R, S;

    public T X => Q;
    public T Y => R;
    public T Z => S;

    public HexBase(T q, T r, T s) 
    {
        // if (q + r + s != 0)  
        // {
        //     throw new ArgumentException("q + r + s must be 0");
        // }

        Q = q;
        R = r;
        S = s;
    }

    public bool Equals(HexBase<T> other)
    {
        if (other is null) return false;
        return Q.Equals(other.Q) && R.Equals(other.R) && S.Equals(other.S);
    }

    public override bool Equals(Object other)
    {   
        if (other is HexBase<T> hex) { 
            return Equals(hex);
        }
        return false;
    }

    public override int GetHashCode() {
        return Q.GetHashCode() ^ R.GetHashCode() ^ S.GetHashCode();
    }

    public static bool operator ==(HexBase<T> lhs, HexBase<T> rhs) {
        return lhs.Equals(rhs);
    }

    public static bool operator !=(HexBase<T> lhs, HexBase<T> rhs) {
        return !lhs.Equals(rhs);
    }
}
using System;

public class FractionalHex : HexBase<double>
{
    public FractionalHex(double q, double r, double s) : base(q, r, s) { }
    public FractionalHex(double q, double r): base(q, r, -q-r) { }

    public Hex HexRound() {
        var q = (int) Math.Round(Q);
        var r = (int) Math.Round(R);
        var s = (int) Math.Round(S);

        var qDiff = Math.Abs(q - Q);
        var rDiff = Math.Abs(r - R);
        var sDiff = Math.Abs(s - S);

        if (qDiff > rDiff && qDiff > sDiff) {
            q = -r-s;
        } 
        else if (rDiff > sDiff)
        {
            r = -q-s;
        }
        else
        {
            s = -q-r;
        }
        return new Hex(q, r, s);
    }
}
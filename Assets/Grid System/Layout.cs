using System;
using System.Collections.Generic;
public struct Layout 
{
    public readonly Orientation Orientation;
    public readonly Point Size;
    public readonly Point Origin;

    public Layout(Orientation ortientation, Point size, Point origin)
    {
        Orientation = ortientation;
        Size = size;
        Origin = origin;
    }

    public Point HexToPixel(Hex hex) {
        var o = Orientation;
        var x = (o.f0 * hex.Q + o.f1 * hex.R) * Size.X;
        var y = (o.f2 * hex.Q + o.f3 * hex.R) * Size.Y;
        return new Point(x + Origin.X, y + Origin.Y);
    }

    public FractionalHex PixelToHex(Point point) {
        var o = Orientation;
        var x = (point.X - Origin.X) / Size.X;
        var y = (point.Y - Origin.Y) / Size.Y;
        var q = o.b0 * x + o.b1 * y;
        var r = o.b2 * x + o.b3 * y;
        return new FractionalHex(q, r);
    }

    public Point HexCornerOffset(int corner) {
        var o = Orientation;
        var angle = 2.0 * Math.PI * (o.StartAngle - corner) / 6.0;
        return new Point(Size.X * Math.Cos(angle), 
                         Size.Y * Math.Sin(angle));
    }
    
    public List<Point> PlygonCorners(Hex hex) {
        var corners = new List<Point>();
        var center = HexToPixel(hex);
        for (var i = 0; i < 6; i++)
        {
            var offset = HexCornerOffset(i);
            corners.Add(new Point(center.X + offset.X, 
                                  center.Y + offset.Y));
        }
        return corners;
    }
}
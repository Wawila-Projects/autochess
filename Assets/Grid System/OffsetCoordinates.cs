[System.Serializable]
public struct OffsetCoordinates 
{
    public int Column;
    public int Row;

    private OffsetCoordinates(int col, int row) 
    {
        Column = col;
        Row = row;
    }

    public static OffsetCoordinates EvenFlatOffset(Hex hex) => FlatOffset(1, hex);
    public static OffsetCoordinates OddFlatOffset(Hex hex) =>  FlatOffset(-1, hex);
    public static Hex EvenFlatOffset(OffsetCoordinates coord) => FlatOffset(1, coord);
    public static Hex OddFlatOffset(OffsetCoordinates coord) =>  FlatOffset(-1, coord);

    public static OffsetCoordinates EvenPointyOffset(Hex hex) => PointyOffset(1, hex);
    public static OffsetCoordinates OddPointyOffset(Hex hex) =>  PointyOffset(-1, hex);
    public static Hex EvenPointyOffset(OffsetCoordinates coord) => PointyOffset(1, coord);
    public static Hex OddPointyOffset(OffsetCoordinates coord) =>  PointyOffset(-1, coord);

    private static OffsetCoordinates FlatOffset(int offset, Hex hex) 
    {
        var col = hex.Q;
        var row = hex.R + (int)((col + offset * (col & 1)) / 2);
        return new OffsetCoordinates(col, row);
    }

    private static Hex FlatOffset(int offset, OffsetCoordinates coord) 
    {
        int q = coord.Column;
        int r = coord.Row - (int)((q + offset * (q & 1)) / 2);
        return new Hex(q, r);
    }

    static public OffsetCoordinates PointyOffset(int offset, Hex hex)
    {
        var row = hex.R;
        var col = hex.Q + (int)((row + offset * (row & 1)) / 2);
        return new OffsetCoordinates(col, row);
    }

    private static Hex PointyOffset(int offset, OffsetCoordinates coord) 
    {
        int r = coord.Row;
        int q = coord.Column - (int)((r + offset * (r & 1)) / 2);
        return new Hex(q, r);
    }
}
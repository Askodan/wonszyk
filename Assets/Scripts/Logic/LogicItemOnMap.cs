
using UnityEngine;
public class LogicItemOnMap
{
    private Vector2Int position;
    public Vector2Int Position { get { return position; } set { position = value; } }
    public LogicItemOnMap(Vector2Int position)
    {
        this.Position = position;
    }
    public LogicItemOnMap(LogicItemOnMap original)
    {
        this.Position = original.position;
    }
    static public Vector2Int[] Items2Vec(LogicItemOnMap[] items)
    {
        var positions = new Vector2Int[items.Length];
        int i = 0;
        foreach (var item in items)
        {
            positions[i] = item.position;
            i++;
        }
        return positions;
    }
    static public LogicItemOnMap[] Vec2Items(Vector2Int[] positions)
    {
        var items = new LogicItemOnMap[positions.Length];
        int i = 0;
        foreach (var position in positions)
        {
            items[i] = new LogicItemOnMap(position);
            i++;
        }
        return items;
    }
    virtual public void LaserHit(LogicMap LM)
    {

    }
    override public string ToString()
    {
        string result = "(" + Position.x + "," + Position.y + ")";
        return result;
    }
    virtual public void PlayerHit(LogicWonsz player, LogicMap LM)
    {

    }
    public LogicItemOnMap() { }
    virtual public int BytesLength { get { return 2; } }
    virtual public byte[] ToBytes()
    {
        byte[] result = new byte[BytesLength];
        result[0] = (byte)Position.x;
        result[1] = (byte)Position.y;
        return result;
    }
    virtual public void LoadFromBytes(byte[] bytes)
    {
        Position = new Vector2Int((int)bytes[0], (int)bytes[1]);
    }
}

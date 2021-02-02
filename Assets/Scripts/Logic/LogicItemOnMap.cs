
using UnityEngine;
public class LogicItemOnMap
{
    private Vector2Int position;
    public Vector2Int Position { get {return position;} set {position = value;} }
    
    public LogicItemOnMap(Vector2Int position)
    {
        this.Position = position;
    }
    public LogicItemOnMap(LogicItemOnMap original){
        this.Position = original.position;
    }
    static public Vector2Int[] Items2Vec(LogicItemOnMap[] items){
        var positions = new Vector2Int[items.Length];
        int i = 0;
        foreach(var item in items){
            positions[i] = item.position;
            i++;
        }
        return positions;
    }
    static public LogicItemOnMap[] Vec2Items(Vector2Int[] positions){
        var items = new LogicItemOnMap[positions.Length];
        int i = 0;
        foreach(var position in positions){
            items[i] = new LogicItemOnMap(position);
            i++;
        }
        return items;
    }
    virtual public void LaserHit(LogicMap LM){

    }
    public override string ToString()
    {
        string result = "("+Position.x+","+Position.y+")";
        return result;
    }
}
        
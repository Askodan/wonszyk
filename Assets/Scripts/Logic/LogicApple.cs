
using UnityEngine;

public class LogicApple : LogicItemOnMap
{
    public LogicApple(Vector2Int position) : base(position)
    {

    }
    
    public LogicApple(LogicWonszPart position) : base(position)
    {
        
    }
    
    override public void LaserHit(LogicMap LM){

    }
}
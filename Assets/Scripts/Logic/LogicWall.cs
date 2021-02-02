
using UnityEngine;

public class LogicWall : LogicItemOnMap
{
    bool stillStanding = true;
    public bool StillStanding { get { return stillStanding; } set { stillStanding = value; } }
    public LogicWall(Vector2Int position) : base(position)
    {
        
    }
    
    public LogicWall(LogicWonszPart position) : base(position)
    {
        
    }
    
    override public void LaserHit(LogicMap LM){
        stillStanding = false;
    }
}
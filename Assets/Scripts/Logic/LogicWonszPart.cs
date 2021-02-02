
using UnityEngine;
public class LogicWonszPart: LogicItemOnMap
{
    LogicWonsz master;
    public LogicWonszPart(LogicWonsz master, Vector2Int position) : base(position)
    {

    }
    
    public LogicWonszPart(LogicWonszPart original): base(original){
        master = original.master;
    }
    override public void LaserHit(LogicMap LM){

    }
}

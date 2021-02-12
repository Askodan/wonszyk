
using UnityEngine;

public class LogicWall : LogicItemOnMap
{
    public LogicWall(Vector2Int position) : base(position)
    {

    }

    public LogicWall(LogicWonszPart position) : base(position)
    {

    }

    override public void LaserHit(LogicMap LM)
    {
        LM.Walls.Remove(this);
        Debug.Log("wall hit with laser");
    }
    override public void PlayerHit(LogicWonsz player, LogicMap LM)
    {
        Debug.Log("wall hit with player");
        player.Collide = true;
        LM.SetChangeLength(player, -1);
    }
}
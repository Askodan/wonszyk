
using UnityEngine;
public class LogicWonszPart : LogicItemOnMap
{
    LogicWonsz master;
    Vector2Int oldPosition;
    public Vector2Int OldPosition { get { return oldPosition; } }
    public LogicWonszPart(LogicWonsz master, Vector2Int position) : base(position)
    {
        this.master = master;
    }
    public LogicWonszPart(LogicWonszPart original) : base(original)
    {
        master = original.master;
    }
    override public void LaserHit(LogicMap LM)
    {
        Debug.Log("wonsz hit with laser");
        var leftParts = master.Cut(this, LM.Data.minLength);
        if (leftParts.Length > LM.Data.lenStillApples)
        {
            foreach (var part in leftParts)
            {
                LM.Walls.Add(new LogicWall(part));
            }
        }
        else
        {
            foreach (var part in leftParts)
            {
                LM.Apples.Add(new LogicApple(part));
            }
        }
    }
    override public void PlayerHit(LogicWonsz player, LogicMap LM)
    {
        Debug.Log("wonsz hit with player");
        player.Collide = true;
        LM.SetChangeLength(player, -1);
    }
    public void SavePosition()
    {
        oldPosition = new Vector2Int(Position.x, Position.y);
    }
    public void LoadPosition()
    {
        Position = oldPosition;
    }

}

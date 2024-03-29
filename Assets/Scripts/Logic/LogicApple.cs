
using UnityEngine;

public class LogicApple : LogicItemOnMap
{
    public LogicApple(Vector2Int position) : base(position)
    {

    }

    public LogicApple(LogicWonszPart position) : base(position)
    {

    }

    override public void LaserHit(LogicMap LM)
    {
        Debug.Log("apple hit with laser");
    }
    override public void PlayerHit(LogicWonsz player, LogicMap LM)
    {
        if (LM.IsCurrentApple(this))
        {
            player.Ate = EatenApple.normal;
        }
        else
        {
            player.Ate = EatenApple.players;
            LM.Apples.Remove(this);
        }
        LM.SetChangeLength(player, 1);
        Debug.Log("apple hit with player");
    }
    public LogicApple() { }
}

using UnityEngine;

public class LogicWall : LogicItemOnMap
{

    uint playerID;
    public uint PlayerID { get { return playerID; } }
    public LogicWall(Vector2Int position, uint playerID) : base(position)
    {
        this.playerID = playerID;
    }

    public LogicWall(LogicWonszPart position, uint playerID) : base(position)
    {
        this.playerID = playerID;
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
    public LogicWall() { }
    override public int BytesLength { get { return 3; } }
    override public byte[] ToBytes()
    {
        byte[] result = new byte[BytesLength];
        result[0] = (byte)Position.x;
        result[1] = (byte)Position.y;
        result[2] = (byte)PlayerID;
        return result;
    }
    override public void LoadFromBytes(byte[] bytes)
    {
        Position = new Vector2Int((int)bytes[0], (int)bytes[1]);
        playerID = (uint)bytes[2];
    }
}
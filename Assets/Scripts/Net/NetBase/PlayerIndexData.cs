using System.IO;
using UnityEngine;
[System.Serializable]
public class PlayerIndexData : MessageData
{
    public int index;
    public PlayerIndexData()
    {
    }
    public PlayerIndexData(byte[] bytes) : base(bytes)
    {
    }

    override public MessageClass GetMessageClass()
    {
        return MessageClass.PlayerIndex;
    }

    override protected void ReadData(BinaryReader reader)
    {
        index = reader.ReadInt32();
    }

    override protected void WriteData(BinaryWriter writer)
    {
        writer.Write(index);
    }
}
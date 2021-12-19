using System.IO;
using UnityEngine;

[System.Serializable]
public class HeaderData : MessageData
{
    public int messageID;
    public int size;
    public HeaderData() { }
    public HeaderData(byte[] data) : base(data) { }
    public HeaderData(int messageID_, int size_)
    {
        messageID = messageID_;
        size = size_;
    }
    protected override void ReadData(BinaryReader reader)
    {
        messageID = reader.ReadInt32();
        size = reader.ReadInt32();
    }
    protected override void WriteData(BinaryWriter writer)
    {
        writer.Write(messageID);
        writer.Write(size);
    }
}

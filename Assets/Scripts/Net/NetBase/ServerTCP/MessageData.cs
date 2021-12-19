using System.IO;
using UnityEngine;


[System.Serializable]
public abstract class MessageData
{
    public MessageData() { }
    public MessageData(byte[] bytes)
    {
        var reader = new BinaryReader(new MemoryStream(bytes));
        ReadData(reader);
    }
    virtual public MessageClass GetMessageClass()
    {
        return MessageClass.Unknown;
    }
    public byte[] ToArray()
    {
        MemoryStream MS = new MemoryStream();
        var writer = new BinaryWriter(MS);
        WriteData(writer);
        return MS.ToArray();
    }

    abstract protected void ReadData(BinaryReader reader);
    abstract protected void WriteData(BinaryWriter writer);
}

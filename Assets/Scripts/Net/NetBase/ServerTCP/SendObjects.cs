using System.Net.Sockets;
using UnityEngine;
delegate void SendObjectWork(SendObject SO);
class SendObject
{
    public SendObject(int buffer_size, Socket socket = null, int messageID = 0)
    {
        Debug.Log(messageID.ToString() + " " + buffer_size.ToString());
        buffer = new byte[buffer_size];
        workSocket = socket;
        MessageID = messageID;
        missingBytes = buffer.Length;
    }
    public SendObjectWork doWork { get; set; }
    public Socket workSocket { get; private set; }
    public int MessageID { get; private set; }
    public byte[] buffer { get; private set; }
    public int missingBytes { get; private set; }
    public int readBytes { get { return buffer.Length - missingBytes; } }
    public void NotifyReadBytes(int bytesRead)
    {
        missingBytes -= bytesRead;
    }
}


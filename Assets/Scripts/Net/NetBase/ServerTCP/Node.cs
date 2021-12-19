using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

public delegate void OnZeroBytesReceived(Socket client);
public abstract class Node
{
    public Node(MessageAnalyzer factory)
    {
        messageAnalyzer = factory;
    }
    protected Socket mySocket;
    private MessageAnalyzer messageAnalyzer;
    public bool IsConnected { get { return mySocket != null && mySocket.Connected; } }
    protected OnZeroBytesReceived onZeroBytesReceived;

    virtual public bool SendData(MessageData data)
    {
        if (IsConnected)
        {
            Send(mySocket, (int)data.GetMessageClass(), data.ToArray());
            return true;
        }
        return false;
    }

    private struct BodyData
    {
        public BodyData(Socket socket_, byte[] data_)
        {
            socket = socket_;
            data = data_;
        }
        public Socket socket;
        public byte[] data;
    }
    protected void Send(Socket socket, int messageID, byte[] byteData)
    {
        var header_data = new HeaderData(messageID, byteData.Length).ToArray();
        var context = new BodyData(socket, byteData);
        socket.BeginSend(header_data, 0, header_data.Length, 0, new AsyncCallback(SendBody), context);
    }
    private static void SendBody(IAsyncResult ar)
    {
        try
        {
            BodyData context = (BodyData)ar.AsyncState;
            int bytesSent = context.socket.EndSend(ar);
            context.socket.BeginSend(context.data, 0, context.data.Length, 0, new AsyncCallback(SendEnd), context.socket);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
    private static void SendEnd(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            int bytesSent = socket.EndSend(ar);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    protected void Receive(Socket client)
    {
        SendObject sentObject = new SendObject(new HeaderData().ToArray().Length, client);
        sentObject.doWork = HandleHeader;
        client.BeginReceive(sentObject.buffer, 0, sentObject.buffer.Length, 0, new AsyncCallback(ReceiveCallback), sentObject);
    }
    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            SendObject receivedObject = (SendObject)ar.AsyncState;
            Socket client = receivedObject.workSocket;
            int bytesRead = client.EndReceive(ar);
            receivedObject.NotifyReadBytes(bytesRead);
            if (bytesRead == 0)
            {
                onZeroBytesReceived(client);
            }
            if (receivedObject.missingBytes == 0)
            {
                receivedObject.doWork(receivedObject);
            }
            else
            {
                client.BeginReceive(receivedObject.buffer, receivedObject.readBytes, receivedObject.missingBytes, 0, new AsyncCallback(ReceiveCallback), receivedObject);
                // Debug.Log("Came " + bytesRead.ToString() + " expected " + receivedObject.buffer.Length.ToString() + " missed " + receivedObject.missingBytes + " already have " + receivedObject.readBytes);
            }
            if (receivedObject.missingBytes < 0)
            {
                Debug.Log("jakaś tragedia " + receivedObject.missingBytes);
            }
        }
        catch (System.ObjectDisposedException e)
        {
            Debug.Log(e.ToString());
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
    private void HandleHeader(SendObject receivedObject)
    {
        var hd = new HeaderData(receivedObject.buffer);
        SendObject sentObject = new SendObject(hd.size, receivedObject.workSocket, hd.messageID);
        sentObject.doWork = HandleData;
        receivedObject.workSocket.BeginReceive(sentObject.buffer, 0, sentObject.buffer.Length, 0, new AsyncCallback(ReceiveCallback), sentObject);
    }
    private void HandleData(SendObject receivedObject)
    {
        messageAnalyzer.WorkWithMessage(receivedObject.MessageID, receivedObject.buffer);
        Receive(receivedObject.workSocket);
    }
}
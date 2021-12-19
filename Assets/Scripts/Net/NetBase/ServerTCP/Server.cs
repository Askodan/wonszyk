using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;


public class Server : Node
{
    //Socket mainServer;
    List<Socket> clients;
    public Server(MessageAnalyzer analyzer) : base(analyzer)
    {
        onZeroBytesReceived = OnZeroBytes;
        clients = new List<Socket>(10);
    }
    public void StartListening(int port = 11000)
    {
        IPAddress ipAddress = IPAddress.Parse("0.0.0.0");//ipHostInfo.AddressList[4];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
        mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            mySocket.Bind(localEndPoint);
            mySocket.Listen(100);
            Debug.Log("Waiting for a connection...");
            mySocket.BeginAccept(new AsyncCallback(AcceptCallback), mySocket);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
    public void Shutdown()
    {
        foreach (var Client in clients)
        {
            Client.Shutdown(SocketShutdown.Both);
            Client.Disconnect(false);
            Client.Close();
        }
        clients.Clear();
        // mySocket?.Shutdown(SocketShutdown.Both);
        mySocket?.Close();
    }
    override public bool SendData(MessageData data)
    {
        if (clients.Count > 0)
        {
            foreach (var Client in clients)
            {
                Send(Client, (int)data.GetMessageClass(), data.ToArray());
            }
            return true;
        }
        return false;
    }
    public int GetNumberOfClients
    {
        get { return clients.Count; }
    }
    private void AcceptCallback(IAsyncResult ar)
    {
        Socket mainServer = (Socket)ar.AsyncState;
        Socket client = mainServer.EndAccept(ar);
        clients.Add(client);
        Receive(client);

        mainServer.BeginAccept(new AsyncCallback(AcceptCallback), mainServer);
    }
    private void OnZeroBytes(Socket client)
    {
        clients.Remove(client);
        client.Shutdown(SocketShutdown.Both);
        client.Close();
    }
}

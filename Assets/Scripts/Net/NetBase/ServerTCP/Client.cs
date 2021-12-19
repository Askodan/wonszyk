using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
// System.Threading; 
using System.IO;

public delegate IEnumerator OnFailedConnection();
public class Client : Node
{
    public OnFailedConnection OnFailedConnection;
    public Client(MessageAnalyzer analyzer) : base(analyzer)
    {
        onZeroBytesReceived = OnZeroBytes;
    }

    public void ConnectToServer(String Ip, int Port = 11000)
    {
        try
        {
            IPAddress ipAddress = IPAddress.Parse(Ip);//ipHostInfo.AddressList[0];  
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, Port);

            mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mySocket.BeginConnect(remoteEP, new AsyncCallback(StartClientCallback), mySocket);

        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
    public void CloseConnection()
    {
        mySocket?.Shutdown(SocketShutdown.Both);
        mySocket?.Close();
    }
    private void StartClientCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            Debug.Log("Socket connected to " + socket.RemoteEndPoint.ToString());
            Receive(socket);
        }
        catch (Exception e)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(OnFailedConnection());
            Debug.LogError(e.ToString());
        }
    }
    private void OnZeroBytes(Socket client)
    {
        CloseConnection();
    }
}

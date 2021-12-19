using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionTest : MonoBehaviour
{
    // Start is called before the first frame update
    Server MyServer;
    Client MyClient;
    MessageAnalyzer MA;
    bool IsServer;

    [SerializeField]
    Text MessagePrinter;
    [SerializeField]
    Text NumberOfClients;
    [SerializeField]
    Toggle IsConnected;
    [SerializeField]
    Toggle IsAlive;

    void Start()
    {
        MA = new MessageAnalyzer();
        MA.OnPlayerIndex += GotData;
    }
    public void MakeServer()
    {
        MyServer = new Server(MA);
        MyServer.StartListening();
        IsServer = true;
    }
    public void Shutdown()
    {
        MyServer?.Shutdown();
        MyServer = null;
    }
    public void ConnectServer(InputField IPHolder)
    {
        MyClient = new Client(MA);
        MyClient.OnFailedConnection = FailedToConnect;
        MyClient.ConnectToServer(IPHolder.text);
        IsServer = false;
    }
    public void Disconnect()
    {
        MyClient?.CloseConnection();
    }
    public void SendNumber(InputField NumberHolder)
    {
        var PID = new PlayerIndexData();
        PID.index = int.Parse(NumberHolder.text);
        if (IsServer)
        {
            MyServer.SendData(PID);
        }
        else
        {
            MyClient.SendData(PID);
        }
    }
    IEnumerator GotData(PlayerIndexData Data)
    {
        string Name = IsServer ? "Serwer" : "Klient";
        Debug.Log(Name + " otrzymał wiadomość " + Data.index);
        MessagePrinter.text = Data.index.ToString();
        yield return null;
    }
    IEnumerator FailedToConnect()
    {
        Debug.Log("Nie udało się połączyć");
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        IsConnected.isOn = MyClient != null && MyClient.IsConnected;
        IsAlive.isOn = MyServer != null;
        NumberOfClients.text = MyServer?.GetNumberOfClients.ToString();
    }
}

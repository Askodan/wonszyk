using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerDataDisplay : MonoBehaviour
{
    private WonszykServerData target;
    [SerializeField] private NumberEditor NumPlayer;
    [SerializeField] private NumberEditor MapSize;
    [SerializeField] private NumberEditor MinLength;
    [SerializeField] private NumberEditor StartLength;
    [SerializeField] private NumberEditor PointsToEnd;
    [SerializeField] private NumberEditor FramesStopped;
    public InputField ipAddress = null;
    public InputField portNumber = null;
    private void Start()
    {
        target = (WonszykServerData)WonszykServerData.Instance;
        target.LoadData();
        NumPlayer.Value = target.PlayersCountToStart;
        MapSize.Value = target.mapSize;
        MinLength.Value = target.minLength;
        StartLength.Value = target.startLength;
        PointsToEnd.Value = target.pointsToEnd;
        FramesStopped.Value = target.framesStopped;
        ipAddress.text = target.ip;
        portNumber.text = target.port;
    }
    public void setNumPlayers()
    {
        target.PlayersCountToStart = NumPlayer.Value;
        target.SaveData();
    }
    public void setMapSize()
    {
        target.mapSize = MapSize.Value;
        target.SaveData();
    }
    public void setMinLength()
    {
        target.minLength = MinLength.Value;
        target.SaveData();
    }
    public void setStartLength()
    {
        target.startLength = StartLength.Value;
        target.SaveData();
    }
    public void setPointsToEnd()
    {
        target.pointsToEnd = PointsToEnd.Value;
        target.SaveData();
    }
    public void setFramesStopped()
    {
        target.framesStopped = FramesStopped.Value;
        target.SaveData();
    }
    public void setIP()
    {
        target.ip = ipAddress.text;
        target.SaveData();
    }
    public void setPort()
    {
        target.port = portNumber.text;
        target.SaveData();
    }
}

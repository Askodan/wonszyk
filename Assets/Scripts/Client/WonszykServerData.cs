using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class WonszykServerData : KeepAliveBetweenScenes, INotifyPropertyChanged
{
    // points
    public int appleEatenPoints = 1;
    public int shotPoints = -1;
    public int collidePoints = -1;

    public float gameSpeed = 2f;

    // saved
    public int pointsToEnd = 5;
    public int PlayersCountToStart = 2;
    public int mapSize = 10;
    public int minLength = 1;
    public int startLength = 3;
    private int framesStopped = 3;
    public int lenStillApples = 1;

    // other
    public ItemOnMap applePrefab;
    public ItemOnMap playerApplePrefav;
    public string ip = "127.0.0.1";
    public string port = "15937";
    public string clientPort = "15937";

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
            handler(this, e);
    }
    protected void OnPropertyChanged(string propertyName)
    {
        OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }
    public int FramesStopped { get { return framesStopped; } set { framesStopped = value; OnPropertyChanged("FramesStopped"); } }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey("ServerPoints2End"))
        {
            pointsToEnd = PlayerPrefs.GetInt("ServerPoints2End");
        }
        if (PlayerPrefs.HasKey("ServerPlayersCountToStart"))
        {
            PlayersCountToStart = PlayerPrefs.GetInt("ServerPlayersCountToStart");
        }
        if (PlayerPrefs.HasKey("ServerMapSize"))
        {
            mapSize = PlayerPrefs.GetInt("ServerMapSize");
        }
        if (PlayerPrefs.HasKey("ServerMinLength"))
        {
            minLength = PlayerPrefs.GetInt("ServerMinLength");
        }
        if (PlayerPrefs.HasKey("ServerStartLength"))
        {
            startLength = PlayerPrefs.GetInt("ServerStartLength");
        }
        if (PlayerPrefs.HasKey("ServerFramesStopped"))
        {
            FramesStopped = PlayerPrefs.GetInt("ServerFramesStopped");
        }
        if (PlayerPrefs.HasKey("ServerIP"))
        {
            ip = PlayerPrefs.GetString("ServerIP");
        }
        if (PlayerPrefs.HasKey("ServerPORT"))
        {
            port = PlayerPrefs.GetString("ServerPORT");
        }
        if (PlayerPrefs.HasKey("ClientPORT"))
        {
            clientPort = PlayerPrefs.GetString("ClientPORT");
        }
        if (PlayerPrefs.HasKey("ServerLenStillApples"))
        {
            lenStillApples = PlayerPrefs.GetInt("ServerLenStillApples");
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("ServerPoints2End", pointsToEnd);
        PlayerPrefs.SetInt("ServerPlayersCountToStart", PlayersCountToStart);
        PlayerPrefs.SetInt("ServerMapSize", mapSize);
        PlayerPrefs.SetInt("ServerMinLength", minLength);
        PlayerPrefs.SetInt("ServerStartLength", startLength);
        PlayerPrefs.SetInt("ServerFramesStopped", FramesStopped);
        PlayerPrefs.SetString("ServerIP", ip);
        PlayerPrefs.SetString("ServerPORT", port);
        PlayerPrefs.SetString("ClientPORT", clientPort);
        PlayerPrefs.SetInt("ServerLenStillApples", lenStillApples);
        PlayerPrefs.Save();
    }
}

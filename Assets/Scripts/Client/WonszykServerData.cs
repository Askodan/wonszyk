using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonszykServerData : KeepAliveBetweenScenes
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
    public int framesStopped = 3;
    public int lenStillApples = 1;

    // other
    public ItemOnMap applePrefab;
    public ItemOnMap playerApplePrefav;
    public string ip = "127.0.0.1";
    public string port = "15937";

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
            framesStopped = PlayerPrefs.GetInt("ServerFramesStopped");
        }
        if (PlayerPrefs.HasKey("ServerIP"))
        {
            ip = PlayerPrefs.GetString("ServerIP");
        }
        if (PlayerPrefs.HasKey("ServerPORT"))
        {
            port = PlayerPrefs.GetString("ServerPORT");
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
        PlayerPrefs.SetInt("ServerFramesStopped", framesStopped);
        PlayerPrefs.SetString("ServerIP", ip);
        PlayerPrefs.SetString("ServerPORT", port);
        PlayerPrefs.SetInt("ServerLenStillApples", lenStillApples);
        PlayerPrefs.Save();
    }
}

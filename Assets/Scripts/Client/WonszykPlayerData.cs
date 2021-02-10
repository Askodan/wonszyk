using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gender
{
    female = 0, male = 1, other = 2
}
public class WonszykPlayerData : MonoBehaviour
{
    string[] names = { "Wonszyk", "Wonszysław", "Wonsz", "Wonszul", "Wonszan", "Władywonsz", "Dobrowonsz", "Wonszymir", "Wonszodor" };
    public bool persistent = true;
    public string WonszName;
    public Color WonszColor = new Color(0, 0, 0, 1);
    public Gender WonszGender = Gender.other;
    public SteeringEnum WonszSteering = SteeringEnum.PC;
    public int smudgeSteeringMinMovement = 5;
    public float tiltSteeringMinMovement = 0.2f;

    public static WonszykPlayerData Instance = null;
    public bool WonszLocalSteering;
    bool loaded = false;

    private void Awake()
    {
        if (persistent)
        {
            DontDestroyOnLoad(this);
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void LoadData()
    {
        loaded = true;
        if (PlayerPrefs.HasKey("WonszColorR"))
        {
            WonszColor = new Color(PlayerPrefs.GetFloat("WonszColorR"), PlayerPrefs.GetFloat("WonszColorG"), PlayerPrefs.GetFloat("WonszColorB"), 1f);
        }
        if (PlayerPrefs.HasKey("WonszName") && PlayerPrefs.GetString("WonszName").Length > 0)
        {
            WonszName = PlayerPrefs.GetString("WonszName");
        }
        else
        {
            WonszName = names[Random.Range(0, names.Length)];
        }
        if (PlayerPrefs.HasKey("WonszGender"))
        {
            WonszGender = (Gender)PlayerPrefs.GetInt("WonszGender");
        }
        if (PlayerPrefs.HasKey("WonszSteering"))
        {
            WonszSteering = (SteeringEnum)PlayerPrefs.GetInt("WonszSteering");
        }
        if (PlayerPrefs.HasKey("WonszLocalSteering"))
        {
            WonszLocalSteering = PlayerPrefs.GetInt("WonszLocalSteering") == 1;
        }
        if (PlayerPrefs.HasKey("SmudgeSteeringMinMovement"))
        {
            smudgeSteeringMinMovement = PlayerPrefs.GetInt("SmudgeSteeringMinMovement");
        }
        if (PlayerPrefs.HasKey("TiltSteeringMinMovement"))
        {
            tiltSteeringMinMovement = PlayerPrefs.GetFloat("TiltSteeringMinMovement");
        }
    }

    public void SaveData()
    {
        // don't save anything if nothing was loaded
        if (!loaded)
        {
            return;
        }
        PlayerPrefs.SetFloat("WonszColorR", WonszColor.r);
        PlayerPrefs.SetFloat("WonszColorG", WonszColor.g);
        PlayerPrefs.SetFloat("WonszColorB", WonszColor.b);
        PlayerPrefs.SetString("WonszName", WonszName);
        PlayerPrefs.SetInt("WonszGender", (int)WonszGender);
        PlayerPrefs.SetInt("WonszSteering", (int)WonszSteering);
        PlayerPrefs.SetInt("WonszLocalSteering", WonszLocalSteering ? 1 : 0);
        PlayerPrefs.SetInt("SmudgeSteeringMinMovement", smudgeSteeringMinMovement);
        PlayerPrefs.SetFloat("TiltSteeringMinMovement", tiltSteeringMinMovement);
        PlayerPrefs.Save();
    }

    public void CopyTo(WonszykPlayerData another)
    {
        another.WonszName = WonszName;
        another.WonszColor = WonszColor;
        another.WonszGender = WonszGender;
        another.WonszSteering = WonszSteering;
        another.WonszLocalSteering = WonszLocalSteering;
        another.smudgeSteeringMinMovement = smudgeSteeringMinMovement;
        another.tiltSteeringMinMovement = tiltSteeringMinMovement;
    }
}

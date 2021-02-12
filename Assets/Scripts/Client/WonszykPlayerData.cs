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
    public Color WonszMainColor = new Color(0, 0, 0, 1);
    public Color WonszPatternColor = new Color(0, 0, 0, 1);
    public int WonszPattern = 1;
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
        // Wonszyk visuals
        if (PlayerPrefs.HasKey("WonszMainColorR"))
        {
            WonszMainColor = new Color(PlayerPrefs.GetFloat("WonszMainColorR"), PlayerPrefs.GetFloat("WonszMainColorG"), PlayerPrefs.GetFloat("WonszMainColorB"), 1f);
        }
        if (PlayerPrefs.HasKey("WonszPatternColorR"))
        {
            WonszPatternColor = new Color(PlayerPrefs.GetFloat("WonszPatternColorR"), PlayerPrefs.GetFloat("WonszPatternColorG"), PlayerPrefs.GetFloat("WonszPatternColorB"), 1f);
        }
        if (PlayerPrefs.HasKey("WonszPattern"))
        {
            WonszPattern = PlayerPrefs.GetInt("WonszPattern");
        }
        // Player settings
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
        PlayerPrefs.SetFloat("WonszMainColorR", WonszMainColor.r);
        PlayerPrefs.SetFloat("WonszMainColorG", WonszMainColor.g);
        PlayerPrefs.SetFloat("WonszMainColorB", WonszMainColor.b);
        PlayerPrefs.SetFloat("WonszPatternColorR", WonszPatternColor.r);
        PlayerPrefs.SetFloat("WonszPatternColorG", WonszPatternColor.g);
        PlayerPrefs.SetFloat("WonszPatternColorB", WonszPatternColor.b);
        PlayerPrefs.SetInt("WonszPattern", WonszPattern);
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
        another.WonszMainColor = WonszMainColor;
        another.WonszPatternColor = WonszPatternColor;
        another.WonszPattern = WonszPattern;

        another.WonszName = WonszName;
        another.WonszGender = WonszGender;
        another.WonszSteering = WonszSteering;
        another.WonszLocalSteering = WonszLocalSteering;
        another.smudgeSteeringMinMovement = smudgeSteeringMinMovement;
        another.tiltSteeringMinMovement = tiltSteeringMinMovement;
    }
}

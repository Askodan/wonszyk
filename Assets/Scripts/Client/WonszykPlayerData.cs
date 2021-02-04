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
    public string WonszName;
    public Color WonszColor = new Color(0, 0, 0, 1);
    public bool persistent = true;
    public Gender WonszGender = Gender.other;
    public SteeringEnum WonszSteering = SteeringEnum.PC;
    public static WonszykPlayerData Instance = null;
    public bool WonszLocalSteering;

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
        if (PlayerPrefs.HasKey("WonszColorR"))
        {
            WonszColor = new Color(PlayerPrefs.GetFloat("WonszColorR"), PlayerPrefs.GetFloat("WonszColorG"), PlayerPrefs.GetFloat("WonszColorB"), 1f);
        }
        if (PlayerPrefs.HasKey("WonszName"))
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
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat("WonszColorR", WonszColor.r);
        PlayerPrefs.SetFloat("WonszColorG", WonszColor.g);
        PlayerPrefs.SetFloat("WonszColorB", WonszColor.b);
        PlayerPrefs.SetString("WonszName", WonszName);
        PlayerPrefs.SetInt("WonszGender", (int)WonszGender);
        PlayerPrefs.SetInt("WonszSteering", (int)WonszSteering);
        PlayerPrefs.SetInt("WonszLocalSteering", WonszLocalSteering ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void CopyTo(WonszykPlayerData another)
    {
        another.WonszName = WonszName;
        another.WonszColor = WonszColor;
        another.WonszGender = WonszGender;
        another.WonszSteering = WonszSteering;
        another.WonszLocalSteering = WonszLocalSteering;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
public class PlayerDataDisplay : MonoBehaviour
{
    WonszykPlayerData target;
    [SerializeField] InputField NameField;
    [SerializeField] Dropdown GenderChooser;
    [SerializeField] Dropdown SteeringChooser;
    [SerializeField] Toggle LocalSteering;

    private void Start()
    {
        SteeringChooser.ClearOptions();
        SteeringChooser.AddOptions(Steering.Available.Values.Select(x => x.Name).ToList());
        target = WonszykPlayerData.Instance;
        PrepareMenus();
    }
    private void PrepareMenus()
    {
        target.LoadData();
        NameField.text = target.WonszName;

        GenderChooser.value = (int)target.WonszGender;
        SteeringChooser.value = System.Array.IndexOf(Steering.Available.Keys.ToArray(), target.WonszSteering);
        LocalSteering.isOn = target.WonszLocalSteering;
    }
    private void OnEnable()
    {
        if (target)
            PrepareMenus();
    }
    public void SetName()
    {
        target.WonszName = NameField.text;
        target.SaveData();
    }
    public void SetGender()
    {
        target.WonszGender = (Gender)GenderChooser.value;
        target.SaveData();
    }
    public void SetSteering()
    {
        target.WonszSteering = Steering.Available.ToArray()[SteeringChooser.value].Key;
        target.SaveData();
    }
    public void SetLocalSteering()
    {
        target.WonszLocalSteering = LocalSteering.isOn;
        target.SaveData();
    }
}

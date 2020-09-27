﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GUIPlayerData : MonoBehaviour
{
    WonszykPlayerData target;
    [SerializeField] FlexibleColorPicker ColorField;
    [SerializeField] InputField NameField;
    [SerializeField] Dropdown GenderChooser;
    [SerializeField] Dropdown SteeringChooser;
    [SerializeField] Toggle LocalSteering;
    
    private void Start()
    {
        SteeringChooser.ClearOptions();
        SteeringChooser.AddOptions(Steering.Available);
        target = WonszykPlayerData.Instance;
        target.LoadData();
        NameField.text = target.WonszName;
        ColorField.startingColor = target.WonszColor;
        
        GenderChooser.value = (int)target.WonszGender;
        SteeringChooser.value = (int)target.WonszSteering;
        LocalSteering.isOn = target.WonszLocalSteering;
    }
    public void SetName()
    {
        target.WonszName = NameField.text;
        target.SaveData();
    }
    public void SetColor()
    {
        target.WonszColor = ColorField.color;
        target.SaveData();
    }
    public void SetGender()
    {
        target.WonszGender = (Gender)GenderChooser.value;
        target.SaveData();
    }
    public void SetSteering()
    {
        target.WonszSteering = (SteeringEnum)SteeringChooser.value;
        target.SaveData();
    }
    public void SetLocalSteering()
    {
        target.WonszLocalSteering = LocalSteering.isOn;
        target.SaveData();
    }
}
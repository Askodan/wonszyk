using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
public class WonszykDataDisplay : MonoBehaviour
{
    WonszykPlayerData target;
    [SerializeField] FlexibleColorPicker MainColorField;
    [SerializeField] FlexibleColorPicker PatternColorField;
    [SerializeField] NumberEditor Pattern;

    private void Start()
    {
        target = WonszykPlayerData.Instance;
        PrepareMenus();
    }
    private void PrepareMenus()
    {
        target.LoadData();
        MainColorField.startingColor = target.WonszMainColor;
        MainColorField.color = target.WonszMainColor;
        PatternColorField.startingColor = target.WonszPatternColor;
        PatternColorField.color = target.WonszPatternColor;
        Pattern.Value = target.WonszPattern;
    }
    private void OnEnable()
    {
        if (target)
            PrepareMenus();
    }
    public void SetMainColor()
    {
        target.WonszMainColor = MainColorField.color;
        target.SaveData();
    }
    public void SetPatternColor()
    {
        target.WonszPatternColor = PatternColorField.color;
        target.SaveData();
    }
    public void SetPattern()
    {
        target.WonszPattern = Pattern.Value;
        target.SaveData();
    }
}

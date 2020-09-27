using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowNumberOfSlider : MonoBehaviour
{
    [SerializeField]
    Slider slider;
    [SerializeField]
    string prefix;
    [SerializeField]
    Text textField;
    private void Start()
    {
        UpdateText();
    }
    public void UpdateText()
    {
        textField.text = prefix + slider.value.ToString();
    }
}

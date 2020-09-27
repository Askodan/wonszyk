using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class NumberEditor : MonoBehaviour
{
    public int MinValue;
    public int MaxValue;
    [SerializeField] private Button IncreaseButton;
    [SerializeField] private Button DecreaseButton;
    [SerializeField] private Text Title;
    [SerializeField] private InputField ValueShower;
    private int myValue;
    public UnityEvent OnValueChanged;
    public int Value
    {
        get
        {
            return myValue;
        }

        set
        {
            IncreaseButton.interactable = value < MaxValue;
            DecreaseButton.interactable = value > MinValue;

            myValue = Mathf.Clamp(value, MinValue, MaxValue);
            ValueShower.text = myValue.ToString();
            OnValueChanged.Invoke();
        }
    }
    public void SetTitle(string newTitle)
    {
        Title.text = newTitle;
    }
    public void Increase()
    {
        Value += 1;
    }

    public void Decrease()
    {
        Value -= 1;
    }

    public void EditedValue()
    {
        Value = int.Parse(ValueShower.text);
    }
}

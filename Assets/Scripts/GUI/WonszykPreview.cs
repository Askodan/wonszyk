using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WonszykPreview : MonoBehaviour
{
    [SerializeField] FlexibleColorPicker MainColorField;
    [SerializeField] FlexibleColorPicker PatternColorField;
    [SerializeField] NumberEditor Pattern;
    [SerializeField] WonszykOnMap Head;
    [SerializeField] WonszykOnMap Body;
    [SerializeField] WonszykOnMap Tail;

    private void Awake()
    {
        Head.PositionInWonszyk = EnumWonszykPartType.Head;
        Body.PositionInWonszyk = EnumWonszykPartType.Body;
        Tail.PositionInWonszyk = EnumWonszykPartType.Tail;
    }
    private void Update()
    {
        SetWonszyk();
    }
    public void SetWonszyk()
    {
        Head.SetColor(MainColorField.color, PatternColorField.color, Pattern.Value);
        Body.SetColor(MainColorField.color, PatternColorField.color, Pattern.Value);
        Tail.SetColor(MainColorField.color, PatternColorField.color, Pattern.Value);
    }
}

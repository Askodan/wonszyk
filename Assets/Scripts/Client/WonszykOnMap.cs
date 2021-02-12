using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnumWonszykPartType
{
    Head = 0,
    Body = 1,
    Tail = 2
}
[System.Serializable]
public class WonszykPartType
{
    // For nice naming in list in Unity
    public string name;
    public GameObject gameObject;
    public MeshRenderer renderer;
}

public class WonszykOnMap : ItemOnMap
{
    EnumWonszykPartType positionInWonszyk;
    [SerializeField] WonszykPartType[] partTypes;
    [SerializeField] Texture2D[] patterns;
    public EnumWonszykPartType PositionInWonszyk
    {
        get
        {
            return positionInWonszyk;
        }

        set
        {
            positionInWonszyk = value;
            for (int i = 0; i < partTypes.Length; i++)
            {
                partTypes[i].gameObject.SetActive((int)value == i);
            }
        }
    }

    public void SetColor(Color mainColor, Color patternColor, int pattern)
    {
        foreach (var rec in partTypes)
        {
            rec.renderer.material.SetColor("_ColorBase", mainColor);
            rec.renderer.material.SetColor("_ColorPattern", patternColor);
            rec.renderer.material.SetTexture("_PatternTex", PatternBank.Instance.GetPattern(pattern));
        }
    }
}

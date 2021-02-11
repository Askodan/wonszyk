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

    public void SetColor(Color color)
    {
        foreach (var rec in partTypes)
        {
            rec.renderer.material.SetColor("_Color", color);
        }
    }
}

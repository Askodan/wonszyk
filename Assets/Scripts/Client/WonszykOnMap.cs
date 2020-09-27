using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WonszykPosition
{
    Head = 0,
    Body = 1,
    Tail = 2
}

public class WonszykOnMap : ItemOnMap
{
    WonszykPosition positionInWonszyk;
    [SerializeField] GameObject[] positions;
    [SerializeField] MeshRenderer[] colorReceivers;
    public WonszykPosition PositionInWonszyk
    {
        get
        {
            return positionInWonszyk;
        }

        set
        {
            positionInWonszyk = value;
            for(int i = 0; i < positions.Length; i++)
            {
                positions[i].SetActive((int)value == i);
            }
        }
    }

    public void SetColor(Color color)
    {
        foreach(var rec in colorReceivers)
        {
            rec.material.SetColor("_Color", color);
        }
    }
}

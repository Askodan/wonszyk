using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetWonsz
{
    public Vector2Int[] positions;
    public PlayerDirection[] directions;
    public uint playerId;
    public bool shot;
    public bool collide;
    public bool ate;
    public int points;

    public override string ToString()
    {
        string result = "";
        foreach (var pos in positions)
        {
            result += "(" + pos.x + "," + pos.y + ") ";
        }
        result += shot ? "shot " : "not shot ";
        result += ate ? "ate " : "not ate ";
        result += collide ? "hit " : "not hit ";
        result += "points " + points;
        return result;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemOnMap : MonoBehaviour
{
    Map myMap;
    Vector2Int position;
    int ignoreLevel = 0;
    PlayerDirection direction;
    public Vector2Int Position
    {
        get
        {
            return position;
        }

        set
        {
            transform.position = new Vector3(value.x + myMap.SizeTranslate, -IgnoreLevel, value.y + myMap.SizeTranslate);  
            position = value;
        }
    }

    public PlayerDirection Direction
    {
        get
        {
            return direction;
        }

        set
        {
            transform.rotation = Quaternion.Euler(0f, value.Angle(), 0f);
            direction = value;
        }
    }

    public int IgnoreLevel
    {
        get
        {
            return ignoreLevel;
        }

        set
        {
            ignoreLevel = value;
        }
    }

    private void Awake()
    {
        myMap = GameLogic.Instance.map;
        myMap.AddItem(this);
    }

    private void OnDestroy()
    {
        myMap.RemoveItem(this);
    }

    public void CopyPositionAndDirection(ItemOnMap another, bool ignore = false)
    {
        IgnoreLevel = another.IgnoreLevel;
        if (ignore)
        {
            IgnoreLevel += 1;
        }
        Position = another.Position;
        Direction = another.direction;
    }

    public void MoveForward()
    {
        if(IgnoreLevel > 0)
        {
            IgnoreLevel -= 1;
        }
        Position = GetMoveForwardPosition();
    }

    public Vector2Int GetMoveForwardPosition()
    {
        Vector2Int newPos = new Vector2Int(Position.x, Position.y) + direction.ToVector2Int();
        // Repeat on edges
        newPos = KeepOnMap(newPos, myMap.Size);
        return newPos;
    }
   
    static public Vector2Int KeepOnMap(Vector2Int pos, int size)
    {
        return new Vector2Int((int)Mathf.Repeat(pos.x, size), (int)Mathf.Repeat(pos.y, size));
    }
    static public Vector2Int DirectionVector(Vector2Int start, Vector2Int end, int size = 0)
    {
        Vector2Int diff = end - start;
        if (size > 0)
        {
            if (diff.x == size - 1)
                diff.x = -1;
            if (diff.y == size - 1)
                diff.y = -1;
            if (diff.x == -(size - 1))
                diff.x = 1;
            if (diff.y == -(size - 1))
                diff.y = 1;
        }
        else
        {
            if (diff.x > 1)
                diff.x = -1;
            if (diff.y > 1)
                diff.y = -1;
            if (diff.x < -1)
                diff.x = 1;
            if (diff.y < -1)
                diff.y = 1;
        }
        return diff;
    }
    // vector utils
    static public Vector2Int[] ToPoints(ItemOnMap[] items)
    {
        Vector2Int[] result = new Vector2Int[items.Length];
        for(int i=0; i < items.Length; i++)
        {
            result[i] = items[i].Position;
        }
        return result;
    }
    static public string Points2String(Vector2Int[] items)
    {
        string result = "";
        for (int i = 0; i < items.Length; i++)
        {
            result += items[i].x.ToString()+","+items[i].y.ToString();
            if(i!=items.Length-1)
                result += ";";
        }
        return result;
    }
    static public Vector2Int[] String2Points(string items)
    {
        string[] splits = items.Split(';');
        if (splits[0] == "")
            return new Vector2Int[0];
        Vector2Int[] result = new Vector2Int[splits.Length];
        for (int i = 0; i < splits.Length; i++)
        {
            string[] point = splits[i].Split(',');
            result[i] = new Vector2Int(int.Parse(point[0]), int.Parse(point[1]));
        }
        return result;
    }
    static public byte[] Points2Bytes(Vector2Int[] items)
    {
        byte[] result = new byte[items.Length*2];
        for (int i = 0; i < items.Length; i++)
        {
            result[2*i] = (byte)items[i].x;
            result[2*i + 1] = (byte)items[i].y;
        }
        return result;
    }
    static public Vector2Int[] Bytes2Points(byte[] items)
    {
        Vector2Int[] result = new Vector2Int[items.Length/2];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = new Vector2Int((int)(items[2*i]), (int)(items[2 * i+1]));
        }
        return result;
    }
}

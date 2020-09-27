using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerDirection
{
    None = -1,
    Up = 0,
    Down = 2,
    Right = 1,
    Left = 3
}

static class PlayerDirectionMethods
{

    public static PlayerDirection FromVector2Int(Vector2Int s1)
    {
        if (s1.x == 0 && s1.y == 1)
            return PlayerDirection.Up;
        if (s1.x == 0 && s1.y == -1)
            return PlayerDirection.Down;
        if (s1.x == 1 && s1.y == 0)
            return PlayerDirection.Right;
        if (s1.x == -1 && s1.y == 0)
            return PlayerDirection.Left;
        // Jeśli nic nie pasuje zwróć None
        return PlayerDirection.None;
    }

    public static PlayerDirection Opposite(this PlayerDirection direction)
    {
        switch (direction)
        {
            case PlayerDirection.Up:
                return PlayerDirection.Down;
            case PlayerDirection.Down:
                return PlayerDirection.Up;
            case PlayerDirection.Right:
                return PlayerDirection.Left;
            case PlayerDirection.Left:
                return PlayerDirection.Right;
            default:
                return PlayerDirection.None;
        }
    }
    public static float Angle(this PlayerDirection direction)
    {
        switch (direction)
        {
            case PlayerDirection.Up:
                return 0f;
            case PlayerDirection.Down:
                return 180f;
            case PlayerDirection.Right:
                return 90f;
            case PlayerDirection.Left:
                return -90f;
            default:
                return float.NaN;
        }
    }
    public static Vector2Int ToVector2Int(this PlayerDirection direction)
    {
        Vector2Int newPos = new Vector2Int(0, 0);
        switch (direction)
        {
            case PlayerDirection.Up:
                newPos.y += 1;
                break;
            case PlayerDirection.Down:
                newPos.y -= 1;
                break;
            case PlayerDirection.Right:
                newPos.x += 1;
                break;
            case PlayerDirection.Left:
                newPos.x -= 1;
                break;
        }
        return newPos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltSteering : Steering
{
    Vector3 basePosition;
    float minMovement = 0.2f;
    public float MinMovement { get { return minMovement; } set { minMovement = value; } }
    PlayerDirection lastMove = PlayerDirection.None;
    public override void Init()
    {
        base.Init();
        Calibrate();
    }
    public void Calibrate()
    {
        basePosition = Input.acceleration;
    }
    public override PlayerDirection Steer(PlayerDirection unallowed)
    {
        PlayerDirection newOne = SteerTilting();
        if (newOne != PlayerDirection.None)
        {
            if (newOne == lastMove)
            {
                return PlayerDirection.None;
            }
            lastMove = newOne;
        }
        return unallowed != newOne ? newOne : PlayerDirection.None;
    }
    private PlayerDirection SteerTilting()
    {
        var acc = Input.acceleration;
        var delta = acc - basePosition;
        var absDelta = new Vector2(Mathf.Abs(delta.x), Mathf.Abs(delta.y));
        if (Mathf.Max(absDelta.x, absDelta.y) < minMovement)
        {
            lastMove = PlayerDirection.None;
            return PlayerDirection.None;
        }
        if (absDelta.x > absDelta.y)
        {
            if (delta.x > 0)
            {
                return PlayerDirection.Right;
            }
            else
            {
                return PlayerDirection.Left;
            }
        }
        else if (absDelta.x < absDelta.y)
        {
            if (delta.y > 0)
            {
                return PlayerDirection.Up;
            }
            else
            {
                return PlayerDirection.Down;
            }
        }
        return PlayerDirection.None;
    }
}

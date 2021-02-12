using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmudgeSteering : Steering
{
    private float minMovement = 0f;
    public float MinMovement { get { return minMovement; } set { minMovement = value; } }
    public override void Init()
    {
        base.Init();
        minMovement = WonszykPlayerData.Instance.smudgeSteeringMinMovement;
    }
    public override PlayerDirection Steer(PlayerDirection unallowed)
    {
        PlayerDirection newOne = SteerSmudge();
        return unallowed != newOne ? newOne : PlayerDirection.None;
    }
    private PlayerDirection SteerSmudge()
    {
        if (Input.touchCount < 1)
            return PlayerDirection.None;

        var delta = Input.GetTouch(0).deltaPosition;
        var absDelta = new Vector2(Mathf.Abs(delta.x), Mathf.Abs(delta.y));
        if (Mathf.Max(absDelta.x, absDelta.y) < MinMovement)
            return PlayerDirection.None;

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

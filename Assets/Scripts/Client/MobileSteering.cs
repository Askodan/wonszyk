using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileSteering : Steering
{
    bool laser = false;
    PlayerDirection current = PlayerDirection.None;
    public override void Init()
    {
        GameLogic.Instance.laser.onClick.AddListener(laserClick);
    }

    public override PlayerDirection Steer(PlayerDirection unallowed)
    {
        PlayerDirection newOne = SteerTouch();
        return unallowed != newOne ? newOne : PlayerDirection.None;
    }

    public override bool ShootLaser()
    {
        bool temp = laser;
        laser = false;
        return temp; 
    }

    void laserClick()
    {
        laser = true;
    }
    PlayerDirection SteerTouch()
    {
        int w = Screen.width;
        int h = Screen.height;
        if (Input.touchCount == 0)
        {
            return PlayerDirection.None;
        }
        Vector2 pos = Input.GetTouch(0).position - new Vector2(w/2, h/2);
        int min = Mathf.Min(w/2, h/2);
        if (Mathf.Abs(pos.x) > min || Mathf.Abs(pos.y) > min)
        {
            return PlayerDirection.None;
        }
        else
        {
            if (pos.x < pos.y)
            {
                if (pos.x > -pos.y) {
                    return PlayerDirection.Up;
                }else{
                    return PlayerDirection.Left;
                }
            }
            else
            {
                if (pos.x > -pos.y)
                {
                    return PlayerDirection.Right;
                }
                else
                {
                    return PlayerDirection.Down;
                }
            }
        }
    }
}

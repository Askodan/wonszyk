using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSteering : Steering
{
    public override PlayerDirection Steer(PlayerDirection unallowed)
    {
        PlayerDirection newOne = SteerWonszKeyboard();
        return unallowed != newOne ? newOne : PlayerDirection.None;
    }
    public override bool ShootLaser()
    {
        return Input.GetButtonDown("Jump") || base.ShootLaser();
    }
    public PlayerDirection SteerWonszKeyboard()
    {
        if (Input.GetButtonDown("Left"))
            return PlayerDirection.Left;
        if (Input.GetButtonDown("Right"))
            return PlayerDirection.Right;
        if (Input.GetButtonDown("Up"))
            return PlayerDirection.Up;
        if (Input.GetButtonDown("Down"))
            return PlayerDirection.Down;
        return PlayerDirection.None;
    }
}

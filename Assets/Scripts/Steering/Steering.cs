using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SteeringEnum
{
    Mobile,
    PC,
    Help
}

public abstract class Steering : MonoBehaviour
{
    static public List<string> Available = new List<string>() { "Normalne", "PC", "Z pomocą"};
    static public Dictionary<SteeringEnum, System.Type> TypeOfEnum = new Dictionary<SteeringEnum, System.Type>() {
        { SteeringEnum.Mobile, typeof(MobileSteering) },
        { SteeringEnum.PC, typeof(PlayerSteering) },
        { SteeringEnum.Help, typeof(HelpSteering) } };
    public bool is_local = true;
    virtual public PlayerDirection Steer(PlayerDirection unallowed)
    {
        return unallowed.Opposite();
    }
    virtual public bool ShootLaser()
    {
        return false;
    }
    virtual public void Init()
    {

    }
    public PlayerDirection localize(PlayerDirection steer, PlayerDirection oppositeCurrent)
    {
        if (steer == PlayerDirection.None)
            return steer;
        if (oppositeCurrent == PlayerDirection.None)
            return steer;

        int add = 0;
        switch (steer)
        {
            case PlayerDirection.Up:
                add = 0;
                break;
            case PlayerDirection.Down:
                add = 0;
                break;
            case PlayerDirection.Right:
                add = +1;
                break;
            case PlayerDirection.Left:
                add = -1;
                break;
        }
        return (PlayerDirection)(Mathf.Repeat((int)(oppositeCurrent.Opposite())+add, 4));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SteeringEnum
{
    Mobile,
    PC,
    Help,
    Smudge,
    Tilt
}
public class SteeringInfo
{
    public SteeringInfo(string name, string type)
    {
        this.name = name;
        this.type = type;
    }
    public string Name { get { return name; } }
    public string Type { get { return type; } }

    private string name;
    private string type;
}
public abstract class Steering : MonoBehaviour
{
    //static public List<string> Available = new List<string>() { "Normalne", "PC", "Z pomocą" };
    static public Dictionary<SteeringEnum, SteeringInfo> Available = new Dictionary<SteeringEnum, SteeringInfo>() {
        { SteeringEnum.Mobile, new SteeringInfo("Normalne", "MobileSteering") },
        { SteeringEnum.PC, new SteeringInfo("PC", "PlayerSteering") },
        { SteeringEnum.Help, new SteeringInfo("Z pomocą", "HelpSteering") },
        { SteeringEnum.Tilt, new SteeringInfo("Przechyłowe", "TiltSteering") },
        { SteeringEnum.Smudge, new SteeringInfo("Smyraśne", "SmudgeSteering") }};
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
    public PlayerDirection Localize(PlayerDirection steer, PlayerDirection oppositeCurrent)
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
        return (PlayerDirection)(Mathf.Repeat((int)(oppositeCurrent.Opposite()) + add, 4));
    }
}

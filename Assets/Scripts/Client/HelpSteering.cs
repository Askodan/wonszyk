using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpSteering : MobileSteering
{
    public override void Init()
    {
        base.Init();
        GameLogic.Instance.map.Helper.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOnMap : ItemOnMap
{
    [SerializeField] private Renderer body;
    override public void SetLogicValue(LogicItemOnMap item)
    {
        var logicWall = (LogicWall)item;
        this.Position = item.Position;
        var player = Player.ActivePlayers[logicWall.PlayerID];
        this.SetColor(player.data.WonszMainColor, player.data.WonszPatternColor, player.data.WonszPattern);
    }
    void SetColor(Color mainColor, Color patternColor, int pattern)
    {
        body.material.SetColor("_ColorBase", mainColor);
        body.material.SetColor("_ColorPattern", patternColor);
        body.material.SetTexture("_PatternTex", PatternBank.Instance.GetPattern(pattern));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsMachine : SpaceOnSide<PlayerText>
{
    Dictionary<uint, int> data;
    protected override void Awake()
    {
        data = new Dictionary<uint, int>();
        base.Awake();
    }
    protected override void SetupSize()
    {
        var rt = GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(CalcWidthRight(Screen.width, Screen.height), 0f);
        rt.anchorMax = new Vector2(1f, 1f);
    }

    public void Log(uint who, int what)
    {
        if (data.ContainsKey(who))
        {
            data[who] = what;
        }
        else
        {
            data.Add(who, what);
        }
        ShowAll();
    }

    void ShowAll()
    {
        List<KeyValuePair<uint, int>> myList = new List<KeyValuePair<uint, int>>(data);
        myList.Sort(
            delegate (KeyValuePair<uint, int> firstPair,
            KeyValuePair<uint, int> nextPair)
            {
                return -firstPair.Value.CompareTo(nextPair.Value);
            }
        );
        int i = 0;
        foreach (var el in myList)
        {
            if (i == texts.Length)
                break;
            texts[i].text = Player.ActivePlayers[el.Key].name + " " + el.Value.ToString();
            texts[i].PlayerID = el.Key;
            i++;
        }
    }
}

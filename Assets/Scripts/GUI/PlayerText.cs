using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerText : Text
{
    private uint playerID;
    public uint PlayerID
    {
        get { return playerID; }
        set
        {
            playerID = value;
            InitPlayerMaterial();
            image.material = Player.ActivePlayers[playerID].mywonsz.Head.GetComponentInChildren<Renderer>().material;
        }
    }
    [SerializeField] private Image image;
    float margin = 2;
    private bool init = true;
    void InitPlayerMaterial()
    {
        if (init)
        {
            var i_rt = image.GetComponent<RectTransform>();
            var rt = GetComponent<RectTransform>();
            var s = Mathf.Min(rt.rect.height, rt.rect.width);
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x + s / 2, rt.anchoredPosition.y);
            rt.sizeDelta += new Vector2(-s, 0);
            i_rt.anchoredPosition = new Vector2((-rt.rect.width - s) / 2, 0);
            i_rt.sizeDelta = new Vector2(s - margin, s - margin);
            init = false;
        }
    }

}
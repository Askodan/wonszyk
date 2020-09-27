using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsMachine : MonoBehaviour
{
    [SerializeField] Text text;
    Dictionary<uint, int> data;
    int limit = 15;
    Text[] texts;
    private void Awake()
    {
        data = new Dictionary<uint, int>();
        texts = new Text[limit];
        texts[0] = text;
        text.text = "";
        GetComponent<RectTransform>().anchorMin = new Vector2(((float)Screen.height + ((float)(Screen.width - Screen.height)) / 2f) / ((float)Screen.width), 0f);
        GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
        for (int i = 1; i < limit; i++)
        {
            texts[i] = Instantiate(text, transform);
            (texts[i].GetComponent<RectTransform>() as RectTransform).anchoredPosition3D = new Vector3(0, -28 - (i * 56), 0);
            texts[i].GetComponent<Text>().text = "";
        }

    }

    public void Log(uint who, int what)
    {
        if (data.ContainsKey(who)) { 
            data[who] = what;
        }
        else{
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
        foreach(var el in myList)
        {
            if (i == texts.Length)
                break;
            texts[i].text = Player.ActivePlayers[el.Key].name+" "+el.Value.ToString();
            i++;
        }
    }
}

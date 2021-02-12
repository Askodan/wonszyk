using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsMachine : MonoBehaviour
{
    [SerializeField] PlayerStatsDisplay text;

    [SerializeField] int limit = 12;
    PlayerStatsDisplay[] texts;
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    void SetNumPlayers(int num)
    {
        int showedNum = Mathf.Min(limit, num)+1;
        texts = new PlayerStatsDisplay[showedNum];
        texts[0] = text;
        RectTransform rt = text.GetComponent<RectTransform>();
        int newSize = (int)rt.rect.height;//58 * limit / showedNum);
        for (int i = 0; i < showedNum; i++)
        {
            if (i > 0)
            {
                texts[i] = Instantiate(text, text.transform.parent);
                texts[i].SetStats("", 0, 0, 0, 0, 0);
            }
            (texts[i].GetComponent<RectTransform>() as RectTransform).anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y - (i * newSize)); //rt.anchoredPosition.y - newSize / 2 - (i * newSize));
        }
    }
    public void Log(NetResults[] results)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        SetNumPlayers(results.Length);
        ShowAll(results);
    }

    void ShowAll(NetResults[] results)
    {
        List<KeyValuePair<string, NetResults>> myList = new List<KeyValuePair<string, NetResults>>();
        foreach (var res in results)
        {
            myList.Add(new KeyValuePair<string, NetResults>(Player.ActivePlayers[res.playerId].name, res));
        }
        myList.Sort(
            delegate (KeyValuePair<string, NetResults> firstPair,
            KeyValuePair<string, NetResults> nextPair)
            {
                return -firstPair.Value.points.CompareTo(nextPair.Value.points);
            }
        );

        int i = 1;
        foreach (var el in myList)
        {
            if (i == texts.Length)
                break;
            texts[i].SetStats(el.Key, el.Value.points, el.Value.shots, el.Value.hits, el.Value.meals, 0);
            i++;
        }
        for (; i < texts.Length; i++)
        {
            texts[i].gameObject.SetActive(false);
        }
    }
}

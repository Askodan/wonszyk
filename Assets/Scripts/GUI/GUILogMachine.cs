using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class GUILogMachine : MonoBehaviour
{
    [SerializeField] Text text;
    Queue<string> messages;
    int limit = 19;
    Text[] texts;
    private void Awake()
    {
        messages = new Queue<string>();
        texts = new Text[limit];
        texts[0] = text;
        text.text = "";
        GetComponent<RectTransform>().anchorMax = new Vector2(((float)(Screen.width-Screen.height))/2f/((float)Screen.width), 1f);
        for ( int i = 1; i < limit; i++)
        {
            texts[i] = Instantiate(text, transform);
            (texts[i].GetComponent<RectTransform>()as RectTransform).anchoredPosition3D = new Vector3(0, -28-(i * 56), 0);
            texts[i].GetComponent<Text>().text = "";
        }
        Log("Oczekiwanie na graczy");
    }

    public void Log(string message)
    {
        messages.Enqueue(message);
        if (messages.Count > limit)
        {
            messages.Dequeue();
        }
        //string full = "";
        int i = 0;
        foreach(var m in messages.ToArray())
        {
            texts[i].text = m;
            i++;
        }
        //text.text = full;
    }
}

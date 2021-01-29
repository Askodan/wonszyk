using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class LogMachine : SpaceOnSide
{
    Queue<string> messages;
    protected override void Awake()
    {
        messages = new Queue<string>();
        base.Awake();
        Log("Oczekiwanie na graczy");
    }
    protected override void SetupSize(){
        var rt = GetComponent<RectTransform>();
        rt.anchorMax = new Vector2(CalcWidthLeft(Screen.width, Screen.height), 1f);
        limit = (int)Mathf.Abs(Mathf.Round(rt.rect.height / text.rectTransform.rect.height));
    }
    public void Log(string message)
    {
        messages.Enqueue(message);
        if (messages.Count > limit)
        {
            messages.Dequeue();
        }
        int i = 0;
        foreach(var m in messages.ToArray())
        {
            texts[i].text = m;
            i++;
        }
    }
}

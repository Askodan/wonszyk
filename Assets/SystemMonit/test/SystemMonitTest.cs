using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemMonitTest : MonoBehaviour
{
    [SerializeField]
    SystemMonit SM;
    [SerializeField]
    float interval;
    [SerializeField]
    List<SystemMonit.MessageData> Data;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowAnotherMonit());
    }

    IEnumerator ShowAnotherMonit()
    {
        while (true)
        {
            SM.SetMessage(Data[Random.Range(0, Data.Count)]);
            yield return new WaitForSeconds(interval);
        }
    }
}

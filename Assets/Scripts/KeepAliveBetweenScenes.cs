using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAliveBetweenScenes : MonoBehaviour
{
    public static KeepAliveBetweenScenes Instance = null;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

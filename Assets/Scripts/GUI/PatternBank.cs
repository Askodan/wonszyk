using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternBank : MonoBehaviour
{
    static public PatternBank Instance;
    [SerializeField] private Texture2D[] patterns;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (!Instance)
            Instance = this;
    }
    public Texture2D GetPattern(int id)
    {
        return patterns[id - 1];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInstruction : MonoBehaviour
{
    [SerializeField] private GameObject[] Instructions;
    private void OnEnable()
    {
        if (WonszykPlayerData.Instance)
        {
            for (int i = 0; i < Instructions.Length; i++)
            {
                Instructions[i].SetActive(i == (int)WonszykPlayerData.Instance.WonszSteering);
            }
        }
    }
}

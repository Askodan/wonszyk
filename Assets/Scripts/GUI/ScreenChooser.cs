using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenChooser : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ScreenToChoose;
    private int current = 0;
    private void Start()
    {
        ChangeScreen(0);
    }
    public void ChangeScreen(int num)
    {
        int i = 0;
        current = current + num;
        foreach (GameObject screen in ScreenToChoose)
        {
            screen.SetActive(i == current);
            i++;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && current > 0)
        {
            ChangeScreen(-1);
        }
    }
}

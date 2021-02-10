using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveParamOnDisabled : MonoBehaviour
{
    // this property exists because slider cannot set int property
    public float floatssmm { set { smudgeSteeringMinMovement = (int)value; } }
    public int smudgeSteeringMinMovement { get { return WonszykPlayerData.Instance.smudgeSteeringMinMovement; } set { WonszykPlayerData.Instance.smudgeSteeringMinMovement = value; } }
    [SerializeField] private Slider sliderSmudgeSteeringMinMovement;
    public float tiltSteeringMinMovement { get { return WonszykPlayerData.Instance.tiltSteeringMinMovement; } set { WonszykPlayerData.Instance.tiltSteeringMinMovement = value; } }
    [SerializeField] private Slider sliderTiltSteeringMinMovement;

    private void OnEnable()
    {
        if (WonszykPlayerData.Instance)
        {
            if (sliderSmudgeSteeringMinMovement)
                sliderSmudgeSteeringMinMovement.value = smudgeSteeringMinMovement;
            if (sliderTiltSteeringMinMovement)
                sliderTiltSteeringMinMovement.value = tiltSteeringMinMovement;
        }
    }
    private void OnDisable()
    {
        WonszykPlayerData.Instance.SaveData();
    }
}

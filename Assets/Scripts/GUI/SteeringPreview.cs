using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteeringPreview : MonoBehaviour
{
    [SerializeField] private Steering steering;
    [SerializeField] private Text result;
    void Update()
    {
        var res = steering.Steer(PlayerDirection.None);
        if (res != PlayerDirection.None)
            result.text = res.ToGUIString();
    }
}

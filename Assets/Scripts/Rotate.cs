using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Rotate : MonoBehaviour
{
    [SerializeField] private Vector3 axis;
    [SerializeField] private float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis, speed*Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{
    [SerializeField] private Vector2 scaleRange;
    private float timeAlive;
    public float TimeAlive
    {
        get { return timeAlive; }
        set { timeAlive = value; }
    }
    private float currentTime;
    private Vector3 startScale;
    private void Awake()
    {
        startScale = transform.localScale;
    }

    private void OnEnable()
    {
        currentTime = 0;
    }
    private void Update()
    {
        var scaleProgress = Mathf.Repeat(currentTime / timeAlive, 1f);
        var scaleCoefficient = Mathf.Lerp(scaleRange.x, scaleRange.y, scaleProgress);
        transform.localScale = new Vector3(startScale.x, startScale.y, startScale.z) * scaleCoefficient;
        currentTime += Time.deltaTime;
    }
}

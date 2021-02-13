using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{
    [SerializeField] private Vector2 scaleRange;
    private float cycleTime;
    public float CycleTime
    {
        get { return cycleTime; }
        set { cycleTime = value; }
    }
    public float TickTime { get; set; }
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
        var scaleProgress = Mathf.Clamp01(currentTime / cycleTime);
        var scaleCoefficient = Mathf.Lerp(scaleRange.x, scaleRange.y, scaleProgress);
        transform.localScale = new Vector3(startScale.x, startScale.y, startScale.z) * scaleCoefficient;
        currentTime += Time.deltaTime;
        if (currentTime > cycleTime + 0.5f * TickTime)
        {
            currentTime -= cycleTime;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleUpEffect : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    public int quantity = 3;
    public float radius = 0.5f;
    private List<GameObject> generated = new List<GameObject>();
  
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < quantity; i++)
        {
            generated.Add(GameObject.Instantiate(prefab, transform));
            float alfa = (float)(i) * 360f /(float) quantity*Mathf.Deg2Rad;
            generated[i].transform.localPosition = radius * new Vector3(Mathf.Cos(alfa), 0, -Mathf.Sin(alfa));
        }
        StartCoroutine(KeepUp());
    }
    
    IEnumerator KeepUp()
    {
        while (true)
        {
            for(int i = 0; i < generated.Count; i++)
            {
                yield return null;
                generated[i].transform.LookAt(Camera.main.transform.position);
            }
        }
    }
}

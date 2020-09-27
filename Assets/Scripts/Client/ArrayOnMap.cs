using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayOnMap : MonoBehaviour
{
    public ItemOnMap[] elements = new ItemOnMap[0];
    [SerializeField] ItemOnMap prefab;

    public void UpdatePositions(Vector2Int[] newpos)
    {
        if (elements.Length != newpos.Length)
        {
            ItemOnMap[] newEls = new ItemOnMap[newpos.Length];
            for (int i = 0; i < Mathf.Max(elements.Length, newpos.Length); i++)
            {
                if (elements.Length > newpos.Length)
                {
                    if (i < newEls.Length)
                    {
                        newEls[i] = elements[i];
                    }
                    else
                    {
                        Destroy(elements[i].gameObject);
                    }
                }
                else
                {
                    if (i < elements.Length)
                    {
                        newEls[i] = elements[i];
                    }
                    else
                    {
                        newEls[i] = Instantiate(prefab);
                    }
                }
            }
            elements = newEls;
        }
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].Position = newpos[i];
        }
    }
}

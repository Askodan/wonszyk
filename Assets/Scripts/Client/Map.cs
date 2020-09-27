using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform mapMainVis;
    [SerializeField] private HashSet<ItemOnMap> items;
    public GameObject Helper;
    private int size = 10;
    float sizeTranslate;
    public float SizeTranslate { get { return sizeTranslate; }}
    public int Size { get { return size; } set {
            size = value;
            sizeTranslate = -((float)size - 1f) / 2f;
            cam.orthographicSize = ((float)size) / 2f;
            mapMainVis.localScale = new Vector3((float)size, 0.1f, (float)size);
            mapMainVis.GetComponent<MeshRenderer>().materials[0].mainTextureScale = new Vector2(size, size);
        } }
    private void Awake()
    {
        items = new HashSet<ItemOnMap>();
    }

    public ItemOnMap IsPlaceFree(Vector2Int position, int ignoreLevel=-1)
    {
        foreach(ItemOnMap item in items)
        {
            if (item.Position.x == position.x && item.Position.y == position.y)
                if (ignoreLevel < 0)
                    return item;
                else
                    if (ignoreLevel == item.IgnoreLevel)
                        return item;
        }
        return null;
    }

    public void AddItem(ItemOnMap item)
    {
        items.Add(item);
    }

    public void RemoveItem(ItemOnMap item)
    {
        items.Remove(item);
    }

    public Vector2Int GetRandomFreePlace()
    {
        Vector2Int newPos;
        do
        {
            newPos = new Vector2Int(Random.Range(0, size), Random.Range(0, size));
        }while (IsPlaceFree(newPos, 0) != null);
        return newPos;
    }
}

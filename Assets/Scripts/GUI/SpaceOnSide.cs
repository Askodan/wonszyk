using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SpaceOnSide : MonoBehaviour
{
    [SerializeField] protected Text text;
    protected Text[] texts;
    protected int limit = 15;

    protected virtual void Awake(){
        SetupSize();
        PrepareDataSpace(limit);
    }

    abstract protected void SetupSize();
    protected void PrepareDataSpace(int spaceLength){
        texts = new Text[spaceLength];
        texts[0] = text;
        text.text = "";
        for ( int i = 1; i < texts.Length; i++)
        {
            texts[i] = Instantiate(text, transform);
            (texts[i].GetComponent<RectTransform>()as RectTransform).anchoredPosition3D = new Vector3(0, -text.rectTransform.rect.height/2-(i * text.rectTransform.rect.height), 0);
            texts[i].GetComponent<Text>().text = "";
        }
    }//(texts[i].GetComponent<RectTransform>() as RectTransform).anchoredPosition = new Vector3(text_rt.anchoredPosition.x, text_rt.anchoredPosition.y - (i * text_rt.sizeDelta.y), 0);
      
    static public float CalcWidthRight(float width, float height){
        float side_space_pix = Mathf.Abs((width - height)/2f);
        return (height + side_space_pix) / width;
    }
    static public float CalcWidthLeft(float width, float height){
        float side_space_pix = Mathf.Abs((width - height)/2f);
        return (side_space_pix) / width;
    }
}
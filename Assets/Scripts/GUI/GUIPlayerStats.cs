using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlayerStats : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text pointsText;
    [SerializeField] Text shotsText;
    [SerializeField] Text hitsText;
    [SerializeField] Text mealsText;
    [SerializeField] Text movedText;

    public void SetStats(string name, int points, int shots, int hits, int meals, int moved)
    {
        nameText.text = name;
        pointsText.text = points.ToString();
        shotsText.text = shots.ToString();
        hitsText.text = hits.ToString();
        mealsText.text = meals.ToString();
        movedText.text = moved.ToString();
    }
}

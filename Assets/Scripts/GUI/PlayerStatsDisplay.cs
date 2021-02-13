using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsDisplay : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text pointsText;
    [SerializeField] Text shotsText;
    [SerializeField] Text shotsHitText;
    [SerializeField] Text hitsText;
    [SerializeField] Text mealsText;

    public void SetStats(string name, int points, int shots, int shotsHit, int hits, int meals)
    {
        nameText.text = name;
        pointsText.text = points.ToString();
        shotsText.text = shots.ToString();
        shotsHitText.text = shots == 0 ? "-" : (100 * shotsHit / shots).ToString() + "%";
        hitsText.text = hits.ToString();
        mealsText.text = meals.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    public int score;
    TMP_Text scoreTxt;

    private void Start()
    {
        scoreTxt = GetComponent<TMP_Text>();
        scoreTxt.text = "Score: 0";
    }

    public void IncreaseScore(int amountToIncrease)
    {
        score += amountToIncrease;
        scoreTxt.text = "Score: " + score.ToString();
        Debug.Log($"Current score now: {score}");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreDisplay;

    private float score = 0f;

    public void IncreaseScore(float amount)
    {
        score += amount;
        scoreDisplay.text = score.ToString();
    }

    public float GetTotalScore()
    {
        return score;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private UIController uiController;
    [SerializeField] private int scoreReward;
    [SerializeField] private int scorePenalty;
    [SerializeField] private int totalScore;
    public int GetTotalScore
    {
        get
        {
            return totalScore;
        }
    }

    [SerializeField] private int deflections;
    public int GetDeflections
    {
        get
        {
            return deflections;
        }
    }


    public void UpdateScore(bool deflected)
    {
        totalScore += deflected ? scoreReward : scorePenalty;
        deflections += deflected ? 1 : 0;
        uiController.UpdateScoreText(totalScore);
    }

    public void ResetScore()
    {
        totalScore = 0;
        deflections = 0;
    }
}

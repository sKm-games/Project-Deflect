using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ScoreController : MonoBehaviour
{
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
        ReferencesController.GetUIController.UpdateScoreText(totalScore);
        if (deflected)
        {
            ReferencesController.GetAchievementController.CheckDeflectedAchievements();
            ReferencesController.GetEventsController.DeflectionEvent(1);
        }

        DebugScreenshots.TakeDebugScreenShoot();
    }

    public void ResetScore()
    {
        totalScore = 0;
        deflections = 0;
    }

    public void GetGameOverInfo(out int t, out int d, out bool nhs)
    {
        t = totalScore;
        
        d = deflections;

        ReferencesController.GetLeaderboardController.GetLeaderboardInfo(TimeScope.AllTime, out int s, out int r);
        nhs = totalScore > s;
    }
}

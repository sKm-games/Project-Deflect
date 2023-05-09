using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

using GooglePlayGames;

public class LeaderboardController : MonoBehaviour
{
    private ILeaderboard dayHighScore;    
    private ILeaderboard weekHighScore;    
    private ILeaderboard allTimeHighScore;

    public void PostScoreToLeaderboard(int s, string id)
    {
#if UNITY_ANDROID
        if (!GooglePlayController.CheckLogin())
        {
            DebugSystem.UpdateDebugText($"UIController: PostScoreToLeaderboard: Failed to post leaderboard, not logged in");
            return;
        }

        Social.ReportScore(s, id, (bool success) =>
        {
            if (success)
            {
                DebugSystem.UpdateDebugText("HighscoreController: PostScoreToLeaderboard: Score posted");
                LoadLeaderboardInfo(id, TimeScope.Week, true); //load to check achievements
            }
            else
            {
                DebugSystem.UpdateDebugText($"HighscoreController: PostScoreToLeaderboard: Score failed to post, using id: {id}");
            }
        });
#endif
    }

    public void GetLeaderboardInfo(TimeScope time, out int s, out int r)
    {
        s = -1000000;
        r = 9999;
        switch (time)
        {
            default:
                break;
            case TimeScope.Today:
                if (dayHighScore != null)
                {
                    s = (int)dayHighScore.localUserScore.value;
                    r = dayHighScore.localUserScore.rank;
                }
                break;
            case TimeScope.Week:
                if (weekHighScore != null)
                {
                    s = (int)dayHighScore.localUserScore.value;
                    r = dayHighScore.localUserScore.rank;
                }
                break;
            case TimeScope.AllTime:
                if (allTimeHighScore != null)
                {
                    s = (int)dayHighScore.localUserScore.value;
                    r = dayHighScore.localUserScore.rank;
                }
                break;
        }
    }

    public void ShowSpecificLeaderboard(string id)
    {
#if UNITY_ANDROID
        if (!GooglePlayController.CheckLogin())
        {
            DebugSystem.UpdateDebugText("HighscoreController: ShowLeaderboard: Not logged in, skip");
            return;
        }
        PlayGamesPlatform.Instance.ShowLeaderboardUI(id);
#endif
    }

    public void ShowCurrentLeaderboard()
    {
#if UNITY_ANDROID        
        if (!GooglePlayController.CheckLogin())
        {
            DebugSystem.UpdateDebugText("HighscoreController: ShowLeaderboard: Not logged in, skip");
            return;
        }
        string id = ReferencesController.GetDifficultyController.GetCurrentDifficulty.LeaderboardID;
        PlayGamesPlatform.Instance.ShowLeaderboardUI(id);
#endif
    }

    public void LoadLeaderboardInfo()
    {
#if UNITY_ANDROID
        if (!GooglePlayController.CheckLogin())
        {
            DebugSystem.UpdateDebugText($"UIController: GetLeaderboardInfo: Failed to get leaderboard info, not logged in");
            return;
        }

        string id = ReferencesController.GetDifficultyController.GetCurrentDifficulty.LeaderboardID;

        dayHighScore = LoadLeaderboardInfo(id, TimeScope.Today);
        weekHighScore = LoadLeaderboardInfo(id, TimeScope.Week);
        allTimeHighScore = LoadLeaderboardInfo(id, TimeScope.AllTime);
#endif
    }

    private ILeaderboard LoadLeaderboardInfo(string id, TimeScope timeScope, bool checkAchive = false) //test for checking rank and score, to be used with achievements in the future
    {
        DebugSystem.UpdateDebugText($"LeaderboardController: LoadLeaderboardInfo {timeScope}: Start");
        
        ILeaderboard lb = PlayGamesPlatform.Instance.CreateLeaderboard();
        lb.id = id;
        lb.timeScope = timeScope;
        
        lb.LoadScores(success =>
        {
            if (success)
            {
                DebugSystem.UpdateDebugText($"LeaderboardController: LoadAllTimeScore: {timeScope} score found with rank {lb.localUserScore.rank} and score {lb.localUserScore.value}");
                if (checkAchive)
                {
                    ReferencesController.GetAchievementController.CheckOtherAchievements("KingOfTheWeek", lb.localUserScore.rank);
                }
            }
            else
            {
                DebugSystem.UpdateDebugText($"LeaderboardController: LoadAllTimeScore: Failed to check {timeScope} score with id {lb.id}");
            }        
        });

        return lb;
    }    

    #region Old
    /*
    private ILeaderboard LoadAllTimeScore(string id) //test for checking rank and score, to be used with achievements in the future
    {
        DebugSystem.UpdateDebugText($"LeaderboardController: LoadAllTimeScore: Start");
        ILeaderboard lb = PlayGamesPlatform.Instance.CreateLeaderboard();
        lb.id = id;
        lb.timeScope = TimeScope.AllTime;         
        lb.LoadScores(success =>
        {
            if (success)
            {
                DebugSystem.UpdateDebugText($"LeaderboardController: LoadAllTimeScore: AllTime score found with rank {lb.localUserScore.rank} and score {lb.localUserScore.value}");                                
            }
            else
            {
                DebugSystem.UpdateDebugText($"LeaderboardController: LoadAllTimeScore: Failed to check AllTime score with id {lb.id}", true);                
            }
        });
        return lb;
    }

    private void LoadWeekScore(string id) //test for checking rank and score, to be used with achievements in the future
    {
        DebugSystem.UpdateDebugText($"LeaderboardController: LoadWeekScore: Start");

        ILeaderboard lb = PlayGamesPlatform.Instance.CreateLeaderboard();
        lb.id = id;

        lb.timeScope = TimeScope.Week;
        lb.LoadScores(success =>
        {
            if (success)
            {
                DebugSystem.UpdateDebugText($"LeaderboardController: LoadWeekScore: Week score found with rank {lb.localUserScore.rank} and score {lb.localUserScore.value}");

            }
            else
            {
                DebugSystem.UpdateDebugText($"LeaderboardController: LoadWeekScore: Failed to check Week score with id {lb.id}");
            }
        });
    }

    private void LoadToDayScore(string id) //test for checking rank and score, to be used with achievements in the future
    {
        DebugSystem.UpdateDebugText($"LeaderboardController: LoadToDayScore: Start");

        ILeaderboard lb = PlayGamesPlatform.Instance.CreateLeaderboard();
        lb.id = id;

        lb.timeScope = TimeScope.Today;
        lb.LoadScores(success =>
        {
            if (success)
            {
                DebugSystem.UpdateDebugText($"LeaderboardController: LoadLeaderBoardScore: ToDay score found with rank {lb.localUserScore.rank} and score {lb.localUserScore.value}");

            }
            else
            {
                DebugSystem.UpdateDebugText($"LeaderboardController: LoadLeaderBoardScore: Failed to check ToDay score with id {lb.id}");
            }
        });
    }*/
    #endregion
}

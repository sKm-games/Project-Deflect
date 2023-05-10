using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

using GooglePlayGames;

public class LeaderboardController : MonoBehaviour
{
    private int dayHighScore;
    private int dayRank;
    private int weekHighScore;
    private int weekRank;
    private int allTimeHighScore;
    private int allTimeRank;

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
            }
            else
            {
                DebugSystem.UpdateDebugText($"HighscoreController: PostScoreToLeaderboard: Score failed to post, using id: {id}");
            }
        });

        CheckAchievements();
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
                s = dayHighScore;
                r = dayRank;
                break;
            case TimeScope.Week:
                s = weekHighScore;
                r = weekRank;
                break;
            case TimeScope.AllTime:
                s = allTimeHighScore;
                r = allTimeRank;
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

    public void LoadAllLeaderboardInfo()
    {
#if UNITY_ANDROID
        string id = ReferencesController.GetDifficultyController.GetCurrentDifficulty.LeaderboardID;

        if (!GooglePlayController.CheckLogin())
        {
            DebugSystem.UpdateDebugText($"UIController: GetLeaderboardInfo: Failed to get leaderboard info, not logged in");
            return;
        }

        /*
        ILeaderboard temp = LoadLeaderboardInfo(id, TimeScope.Today);
        if (temp != null)
        {
            dayHighScore = (int)temp.localUserScore.value;
            dayRank = temp.localUserScore.rank;
        }
        temp = LoadLeaderboardInfo(id, TimeScope.Week);
        if (temp != null)
        {
            weekHighScore = (int)temp.localUserScore.value;
            weekRank = temp.localUserScore.rank;
        }        
        
        temp = LoadLeaderboardInfo(id, TimeScope.AllTime);

        if (temp != null)
        {
            allTimeHighScore = (int)temp.localUserScore.value;
            allTimeRank = temp.localUserScore.rank;
        }*/

        LoadLeaderboardInfo(id, TimeScope.Today,
            callback =>
            {
                if (callback != null)
                {
                    dayHighScore = (int)callback.localUserScore.value;
                    dayRank = callback.localUserScore.rank;
                }
            });

        LoadLeaderboardInfo(id, TimeScope.Week,
            callback =>
            {
                if (callback != null)
                {
                    weekHighScore = (int)callback.localUserScore.value;
                    weekRank = callback.localUserScore.rank;
                }
            });

        LoadLeaderboardInfo(id, TimeScope.AllTime,
            callback =>
            {
                if (callback != null)
                {
                    allTimeHighScore = (int)callback.localUserScore.value;
                    allTimeRank = callback.localUserScore.rank;
                }
            });
#endif
    }

    private void CheckAchievements()
    {
        string id = ReferencesController.GetDifficultyController.GetCurrentDifficulty.LeaderboardID;
        LoadLeaderboardInfo(id, TimeScope.Week,
            callback =>
            {
                if (callback != null)
                {
                    ReferencesController.GetAchievementController.CheckOtherAchievements("KingOfTheWeek", 1);
                }
            });        
    }

    IEnumerator LoadLeaderboardInfo(string id, TimeScope timeScope, System.Action<ILeaderboard> callBack) //test for checking rank and score, to be used with achievements in the future
    {
        DebugSystem.UpdateDebugText($"LeaderboardController: LoadLeaderboardInfo {timeScope}: Start");

        ILeaderboard lb = PlayGamesPlatform.Instance.CreateLeaderboard();
        lb.id = id;
        lb.timeScope = timeScope;

        lb.LoadScores(success =>
        {
            if (success)
            {
                DebugSystem.UpdateDebugText($"LeaderboardController: LoadLeaderboardInfo: {timeScope} score found with rank {lb.localUserScore.rank} and score {lb.localUserScore.value}");
            }
            else
            {
                DebugSystem.UpdateDebugText($"LeaderboardController: LoadLeaderboardInfo: Failed to check {timeScope} score with id {lb.id}");
            }        
        });

        float timeOut = 0;

        while (lb.loading && timeOut < 10f)
        {
            timeOut += Time.deltaTime;           
            yield return null;  
        }

        callBack(lb);
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

using UnityEngine;
using UnityEngine.SocialPlatforms;

using GooglePlayGames;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField] private DifficultyController difficultyController;
    
    public void PostScoreToLeaderboard(int s, string id)
    {
#if UNITY_ANDROID
        if (GooglePlayController.CheckLogin() == 0)
        {
            DebugSystem.UpdateDebugText("HighscoreController: PostScoreToLeaderboard: Not logged in, skip");
            return;
        }
        Social.ReportScore(s, id, (bool success) =>
        {
            if (success)
            {
                DebugSystem.UpdateDebugText("HighscoreController: PostScoreToLeaderboard: Score posted");
                CheckLeaderboardScore(id);
            }
            else
            {
                DebugSystem.UpdateDebugText($"HighscoreController: PostScoreToLeaderboard: Score failed to post, using id: {id}", true);
            }
        });
#endif
    }

    public void ShowSpecificLeaderboard(string id)
    {
#if UNITY_ANDROID
        if (GooglePlayController.CheckLogin() == 0)
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
        string id = difficultyController.GetCurrentDifficulty.LeaderboardID;
        if (GooglePlayController.CheckLogin() == 0)
        {
            DebugSystem.UpdateDebugText("HighscoreController: ShowLeaderboard: Not logged in, skip");
            return;
        }
        PlayGamesPlatform.Instance.ShowLeaderboardUI(id);
#endif

    }

    public void CheckLeaderboardScore(string id)
    {
#if UNITY_ANDROID
        LoadAllTimeScore(id);
        LoadWeekScore(id);
        LoadToDayScore(id);
#endif
    }

    private void LoadAllTimeScore(string id) //test for checking rank and score, to be used with achievements in the future
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
                DebugSystem.UpdateDebugText($"LeaderboardController: LoadAllTimeScore: Failed to check AllTime score with id {lb.id}");
            }
        });
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
    }
}

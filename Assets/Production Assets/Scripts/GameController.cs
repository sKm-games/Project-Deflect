using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    private bool gameIsRunning;
    public bool GameIsRunning
    {
        get
        {
            return gameIsRunning;
        }
        set
        {
            gameIsRunning = value;
        }
    }

    [SerializeField] private int lifes;
    private int activeLifes;
    private int adRetries;


    private void Awake()
    {
        ReferencesController.GetBlockerController.ToggleBlocker(false);

        ReferencesController.GetLevelController.Init();
    }

    private void Start()
    {
        ReferencesController.GetSaveManager.GetSaveInfo();
    }

    public void StartGame(int d)
    {        
        if (!gameIsRunning)
        {
            Debug.Log("Start Game");
            adRetries = 0;
            ReferencesController.GetScoreController.ResetScore();
            ReferencesController.GetDifficultyController.SetDifficutly(d);
            ReferencesController.GetLevelController.SetupLevel(true);
            ReferencesController.GetLeaderboardController.LoadLeaderboardInfo();
            gameIsRunning = true;
                        
            ReferencesController.GetBlockerController.Init(ReferencesController.GetDifficultyController.GetCurrentDifficulty);                        
            ReferencesController.GetUIController.UpdateLevelText(0);

            ReferencesController.GetUIController.UpdateScoreText(0);
            ReferencesController.GetBlockerController.ToggleBlocker(true);
            Time.timeScale = 1;

            ReferencesController.GetEventsController.ModeStartedEvent();
        }
    }

    public void VideoContinue()
    {
        ReferencesController.GetLevelController.SetupLevel(false);
        ReferencesController.GetUIController.ToggleGameOverScreen(false, false);
        ReferencesController.GetBlockerController.ToggleBlocker(true);
        ReferencesController.GetEventsController.RebuildEvent();
        adRetries++;
        gameIsRunning = true;
        
    }

    public void RetryDifficulty()
    {
        StartGame(ReferencesController.GetDifficultyController.GetDifficultyIndex());
        ReferencesController.GetUIController.ToggleGameOverScreen(false, false);
    }

    public void SetLifes(int l)
    {
        lifes = l;
        activeLifes = l;
    }

    public void UpdateLifes(int a)
    {
        if (DebugSystem.InfinitLifeStatic)
        {
            return;
        }

        activeLifes += a;

        if (activeLifes <= 0)
        {            
            GameIsOver(false);
        }
    }

    public void GameIsOver(bool w)
    {        
        gameIsRunning = false;
        ReferencesController.GetBlockerController.ToggleBlocker(false);
        ReferencesController.GetUIController.ToggleGameOverScreen(true, w);
        ReferencesController.GetAchievementController.CheckOtherAchievements("NeverGiveUp!", adRetries);
        
        if (adRetries == 0)
        {
            ReferencesController.GetAchievementController.CheckOtherAchievements("PerfectVictory", activeLifes);
            ReferencesController.GetAchievementController.CheckOtherAchievements("PyrrhicVictory", activeLifes);
        }
        
    }

    public void ReloadGame()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

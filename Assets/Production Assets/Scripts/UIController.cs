using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class UIController : MonoBehaviour
{
    [SerializeField] private MainMenuUIClass mainMenuUI;

    [SerializeField] private GameplayUIClass gameplayUI;
    
    [SerializeField] private GameOverUIClass gameOverUI;

    [SerializeField] private GameObject googlePlayErrorScreen;

    [SerializeField] private TextMeshProUGUI versionText;


    private void OnApplicationFocus(bool focus)
    {
        if (!focus && CheckScreenStatus("gameScreen"))
        {
            TogglePauseScreen(true);
        }
    }

    private void Awake()
    {
        versionText.text = $"v{Application.version}";
        ReferencesController.GetLoadingScreenController.ToggleLoadingScreen(true);     
    }

    private void Start()
    {
        DefaultLayout();
        SetupDiffictiesButtons();
    }

    private void DefaultLayout()
    {
        mainMenuUI.MainMenuHolder.SetActive(true);
        mainMenuUI.StartScreen.SetActive(true);
        mainMenuUI.ModeSelectScreen.SetActive(false);
        mainMenuUI.SettingsScreen.SetActive(false);
        mainMenuUI.GameInfoScreen.SetActive(false);
        mainMenuUI.CreditsScreen.SetActive(false);

        gameplayUI.GameUIHolder.SetActive(false);
        gameplayUI.GameplayScreen.SetActive(false);
        gameplayUI.PauseScreen.SetActive(false);
        gameplayUI.CountdownText.text = "0";
        gameOverUI.GameOverScreen.SetActive(false);
        

        googlePlayErrorScreen.SetActive(false);
        
        Time.timeScale = 1;
        ReferencesController.GetGameController.GameIsRunning = false;
        
    }

    private void SetupDiffictiesButtons()
    {
        for (int i = 0; i < mainMenuUI.ModeSelectButtons.Count; i++)
        {
            mainMenuUI.ModeSelectButtons[i].onClick.RemoveAllListeners();
            mainMenuUI.ModeSelectButtons[i].onClick.AddListener(() => ReferencesController.GetGameController.StartGame(i));
            mainMenuUI.ModeSelectButtons[i].onClick.AddListener(() => ModesToGame());
        }

        for (int i = 0; i < mainMenuUI.ModeLeaderboardButtons.Count; i++)
        {
            mainMenuUI.ModeLeaderboardButtons[i].onClick.RemoveAllListeners();
            int index = i;
            mainMenuUI.ModeLeaderboardButtons[i].onClick.AddListener(() => ShowLeaderboard(index));
        }
    }

    public void ShowLeaderboard(int i)
    {
        string s = ReferencesController.GetDifficultyController.GetDifficultyLeaderboardRef(i);
        ReferencesController.GetLeaderboardController.ShowSpecificLeaderboard(s);
    }

    public void UpdateLeadeboardButtons()
    {
        foreach (Button b in mainMenuUI.ModeLeaderboardButtons)
        {
            b.interactable = GooglePlayController.CheckLogin();
        }
    }

    public void ModesToGame()
    {
        mainMenuUI.ModeSelectScreen.SetActive(false);
        mainMenuUI.MainMenuHolder.SetActive(false);
        gameplayUI.GameUIHolder.SetActive(true);
        gameplayUI.GameplayScreen.SetActive(true);        
        ReferencesController.GetSoundController.PlaySFX("button");
    }

    public void StartScreenToMode()
    {
        mainMenuUI.StartScreen.SetActive(false);
        UpdateLeadeboardButtons();
        ReferencesController.GetSoundController.PlaySFX("button");
        mainMenuUI.ModeSelectScreen.SetActive(true);
    }

    public void SettingToStartScreen()
    {
        mainMenuUI.SettingsScreen.SetActive(false);
        mainMenuUI.StartScreen.SetActive(true);
        ReferencesController.GetSaveManager.AddToSave("", 0, 0);
        ReferencesController.GetSoundController.PlaySFX("button");
    }

    public void PauseToStartScreen()
    {
        ReferencesController.GetInterstitialADController.LoadAd();

        ReferencesController.GetSoundController.StopLoopSFX("alarm", true);

        ReferencesController.GetLevelController.ResetTargets();
        ReferencesController.GetLevelController.StopProgress();
        ReferencesController.GetBlockerController.ToggleBlocker(false);
        ReferencesController.GetGameController.GameIsRunning = false;
        ReferencesController.GetEventsController.ModeQuitEvent();        

        DefaultLayout();
    }

    public void GameOverToStartScreen()
    {
        ReferencesController.GetInterstitialADController.LoadAd();

        ReferencesController.GetLevelController.ResetTargets();
        ReferencesController.GetLevelController.StopProgress();
        ReferencesController.GetBlockerController.ToggleBlocker(false);
        ReferencesController.GetGameController.GameIsRunning = false;

        DefaultLayout();
    }

    public void UpdateCountdownText(int a, bool b = true)
    {
        gameplayUI.CountdownText.gameObject.SetActive(b);
        string s = a > 0 ? a.ToString() : "INCOMING!";
        gameplayUI.CountdownText.text = s;
        gameplayUI.CountdownText.transform.DOPunchScale(new Vector3(0.25f, 0.25f, 0f), 0.5f, 0 , 0);
    }

    public void UpdateScoreText(int s)
    {
        gameplayUI.ScoreText.text = $"Score: {s}";
    }

    public void UpdateLevelText(int l)
    {
        ReferencesController.GetLevelController.GetLevelInfo(out int totalLevels, out string difficulty);
        bool endless = ReferencesController.GetDifficultyController.GetCurrentDifficulty.Mode == ModeEnums.Endless;

        string s = endless ? $"{difficulty}: {(l + 1).ToString("00")}" : $"{difficulty}: {(l + 1).ToString("00")}/{totalLevels}";        
        gameplayUI.LevelText.text = s;
    }

    public void ToggleGameOverScreen(bool b, bool w)
    {
        if (b)
        {
            ReferencesController.GetScoreController.GetGameOverInfo(out int score, out int deflects, out bool newHighScore);
            ReferencesController.GetDifficultyController.GetGameOverInfo(out string diffID, out string leaderID);   
            
            ReferencesController.GetLeaderboardController.PostScoreToLeaderboard(score, leaderID);            
            
            ReferencesController.GetSaveManager.AddToSave(diffID, score, deflects);

            gameOverUI.TitleText.text = w ? "Winner!" : "Game Over!";
            string colorString = ColorUtility.ToHtmlStringRGBA(gameOverUI.HighScoreTextColor);
            Debug.Log($"colorString {colorString}");
            gameOverUI.ScoreText.text = newHighScore ? $"<b><color=#{colorString}>New \nHigh Score</b></color>\nScore \n{score}" : $"\n\nScore \n{score}";

            ReferencesController.GetVideoADController.LoadVideoAD();
            ReferencesController.GetVideoADController.ToggleAdButton(0, !w);
            gameOverUI.LeaderboardButton.interactable = GooglePlayController.CheckLogin();
                        
            if (w)
            {
                ReferencesController.GetEventsController.ModeCompletedEvent();
                ReferencesController.GetAchievementController.CheckModeDoneAchievements();
            }
            else
            {
                ReferencesController.GetEventsController.ModeGameOverEvent();
            }            
        }

        gameOverUI.GameOverScreen.SetActive(b);
        gameplayUI.GameplayScreen.SetActive(!b);
        gameplayUI.PauseScreen.SetActive(false);        
    }

    public void TogglePauseScreen(bool b)
    {
        ReferencesController.GetBlockerController.AllowMove = !b;
        gameplayUI.PauseScreen.SetActive(b);

        if (!b)
        {
            ReferencesController.GetSaveManager.AddToSave("", 0, 0);
        }
        else
        {
            ReferencesController.GetSoundController.StopLoopSFX("alarm", false);
        }

        Time.timeScale = b ? 0 : 1;
        ReferencesController.GetSoundController.PlaySFX("button");
    }

    public bool CheckScreenStatus(string id)
    {
        switch (id)
        {
            case "startScreen":
                return mainMenuUI.StartScreen.activeSelf;
            case "gameScreen":
                return gameplayUI.GameplayScreen.activeSelf;
        }
        DebugSystem.UpdateDebugText($"UIController: CheckScreenStatus: Invalid ID {id}");
        return false;        
    }

    public void ToggleLoadingScreen(bool b)
    {
        ReferencesController.GetLoadingScreenController.ToggleLoadingScreen(b);
    }

    public void ToggleGooglePlayErrorScreen(bool b)
    {
        googlePlayErrorScreen.SetActive(b);
        if (!b)
        {
            ReferencesController.GetSoundController.PlaySFX("button");
        }
    }
}

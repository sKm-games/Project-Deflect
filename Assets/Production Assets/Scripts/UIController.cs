using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class UIController : MonoBehaviour
{
    [SerializeField] private GameController gameController;    
    [SerializeField] private LevelController levelController;
    [SerializeField] private DifficultyController difficultyController;
    [SerializeField] private VideoADController videoADController;
    [SerializeField] private SoundController soundController;
    [SerializeField] private GooglePlayController googlePlayController;    
    [SerializeField] private LoadingScreenController loadingScreenController;    
    public LoadingScreenController GetLoadingScreenController
    {
        get
        {
            return loadingScreenController;
        }
    }
    [SerializeField] private GameObject mainMenuUI;

    [SerializeField] private List<Button> difficultiesModeButton;
    [SerializeField] private List<Button> difficultiesLeaderboardButton;
    [SerializeField] private GameObject pauseWindow;
    private GameObject startScreen;
    private GameObject modeSelectionScreen;
    private GameObject settingScreen;
    [SerializeField] private GameObject gameUI;
    private GameObject gameplayScreen;
    private TextMeshProUGUI gameplayLevelText;
    private TextMeshProUGUI gameplayScoreText;
    private TextMeshProUGUI gameplayCountText;
    private GameObject gameOverScreen;
    private TextMeshProUGUI gameOverTitleText;
    private TextMeshProUGUI gameOverText;

    [SerializeField] private GameObject googlePlayErrorScreen;

    [SerializeField] private TextMeshProUGUI versionText;


    private void Awake()
    {
        versionText.text = $"v{Application.version}";
        loadingScreenController.ToggleLoadingScreen(true);
        
        startScreen = mainMenuUI.transform.GetChild(0).gameObject;
        modeSelectionScreen = mainMenuUI.transform.GetChild(1).gameObject;
        settingScreen = mainMenuUI.transform.GetChild(2).gameObject;

        gameplayScreen = gameUI.transform.GetChild(0).gameObject;
        Transform t = gameplayScreen.transform.GetChild(0);
        gameplayLevelText = t.GetChild(0).GetComponent<TextMeshProUGUI>();
        gameplayScoreText = t.GetChild(1).GetComponent<TextMeshProUGUI>();

        gameplayCountText = gameplayScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        gameOverScreen = gameUI.transform.GetChild(1).gameObject;
        gameOverTitleText = gameOverScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        gameOverText = gameOverScreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>();        
    }

    private void Start()
    {
        DefaultLayout();
        SetupDiffictiesButtons();
    }

    private void DefaultLayout()
    {
        startScreen.SetActive(true);
        modeSelectionScreen.SetActive(false);
        settingScreen.SetActive(false);        

        gameplayScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        pauseWindow.SetActive(false);

        googlePlayErrorScreen.SetActive(false);
        
        Time.timeScale = 1;
        gameController.GameIsRunning = false;
        gameplayCountText.text = "0";
    }

    private void SetupDiffictiesButtons()
    {
        for (int i = 0; i < difficultiesModeButton.Count; i++)
        {
            difficultiesModeButton[i].onClick.RemoveAllListeners();
            difficultiesModeButton[i].onClick.AddListener(() => gameController.StartGame(i));
            difficultiesModeButton[i].onClick.AddListener(() => ModesToGame());
        }

        for (int i = 0; i < difficultiesLeaderboardButton.Count; i++)
        {
            difficultiesLeaderboardButton[i].onClick.RemoveAllListeners();
            int index = i;
            difficultiesLeaderboardButton[i].onClick.AddListener(() => ShowLeaderboard(index));
        }
    }

    public void ShowLeaderboard(int i)
    {
        string s = difficultyController.GetDifficultyLeaderboardRef(i);
        googlePlayController.GetLeaderbordController.ShowSpecificLeaderboard(s);
    }

    public void UpdateLeadeboardButtons()
    {
        foreach (Button b in difficultiesLeaderboardButton)
        {
            b.interactable = GooglePlayController.CheckLogin() == 1 ? true : false;
        }
    }

    public void ModesToGame()
    {
        modeSelectionScreen.SetActive(false);
        gameplayScreen.SetActive(true);
        soundController.PlaySFX("button");
    }

    public void StartScreenToMode()
    {
        startScreen.SetActive(false);
        UpdateLeadeboardButtons();
        soundController.PlaySFX("button");
        modeSelectionScreen.SetActive(true);
    }

    public void SettingToStartScreen()
    {
        settingScreen.SetActive(false);
        startScreen.SetActive(true);
        gameController.GetSaveManager.AddToSave("", 0, 0);
        soundController.PlaySFX("button");
    }

    public void GameToStartScreen()
    {
        levelController.ResetTargets();
        levelController.StopProgress();        
        gameController.GetBlockerController.ToggleBlocker(false);
        gameController.GameIsRunning = false;
        DefaultLayout();
    }

    public void UpdateCountdownText(int a, bool b = true)
    {
        gameplayCountText.gameObject.SetActive(b);
        string s = a > 0 ? a.ToString() : "Incoming!";
        gameplayCountText.text = s;
        gameplayCountText.transform.DOPunchScale(new Vector3(0.25f, 0.25f, 0f), 0.5f, 0 , 0);
    }

    public void UpdateScoreText(int s)
    {
        gameplayScoreText.text = $"Score: {s}";
    }

    public void UpdateLevelText(int l, bool e)
    {        
        levelController.GetLevelInfo(out int totalLevels, out string difficulty);
        string s = e ? $"{difficulty}: {(l + 1).ToString("00")}" : $"{difficulty}: {(l + 1).ToString("00")}/{totalLevels}";        
        gameplayLevelText.text = s;
    }

    public void ToggleGameOverScreen(bool b, bool w, int s)
    {
        if (b)
        {
            gameOverTitleText.text = w ? "Winner!" : "Game Over!";
            gameOverText.text = $"Score: \n{s}";            
        }
        videoADController.LoadVideoAD();
        gameOverScreen.SetActive(b);
        pauseWindow.SetActive(false);
        videoADController.ToggleAdButton(0, true);
    }

    public void TogglePauseScreen(bool b)
    {
        gameController.GetBlockerController.AllowMove = !b;
        pauseWindow.SetActive(b);

        if (!b)
        {
            gameController.GetSaveManager.AddToSave("", 0, 0);   
        }

        Time.timeScale = b ? 0 : 1;
        soundController.PlaySFX("button");
    }

    public bool CheckScreenStatus(string id)
    {
        switch (id)
        {
            case "startScreen":
                return startScreen.activeSelf;
        }
        DebugSystem.UpdateDebugText($"UIController: CheckScreenStatus: Invalid ID {id}");
        return false;        
    }

    public void ToggleLoadingScreen(bool b)
    {
        loadingScreenController.ToggleLoadingScreen(b);
    }

    public void ToggleGooglePlayErrorScreen(bool b)
    {
        googlePlayErrorScreen.SetActive(b);
        if (!b)
        {
            soundController.PlaySFX("button");
        }
    }
}

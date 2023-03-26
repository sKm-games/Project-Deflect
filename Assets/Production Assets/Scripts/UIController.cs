using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private LevelController levelController;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject pauseWindow;
    [SerializeField] private LoadingScreenController loadingScreenController;
    public LoadingScreenController GetLoadingScreenController
    {
        get
        {
            return loadingScreenController;
        }
    }
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
    }

    private void DefaultLayout()
    {
        startScreen.SetActive(true);
        modeSelectionScreen.SetActive(false);
        settingScreen.SetActive(false);        

        gameplayScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        pauseWindow.SetActive(false);
        
        Time.timeScale = 1;
        gameplayCountText.text = "0";
    }

    public void ModesToGame()
    {
        modeSelectionScreen.SetActive(false);
        gameplayScreen.SetActive(true);
    }

    public void SettingToStartScreen()
    {
        settingScreen.SetActive(false);
        startScreen.SetActive(true);
        gameController.GetSaveManager.AddToSave("", 0, 0);
    }

    public void UpdateCountdownText(int a, bool b = true)
    {
        gameplayCountText.gameObject.SetActive(b);
        string s = a > 0 ? a.ToString() : "GO";
        gameplayCountText.text = s;
        gameplayCountText.transform.DOPunchScale(new Vector3(0.25f, 0.25f, 0f), 0.5f, 0 , 0);
    }

    public void UpdateScoreText(int s)
    {
        gameplayScoreText.text = $"Score: {s}";
    }

    public void UpdateLevelText(int l)
    {        
        levelController.GetLevelInfo(out int totalLevels, out string difficulty);        
        gameplayLevelText.text = $"{difficulty}: {(l+1).ToString("00")}/{totalLevels}";
    }

    public void ToggleGameOverScreen(bool b, bool w, int s)
    {
        if (b)
        {
            gameOverTitleText.text = w ? "Winner!" : "Game Over!";
            gameOverText.text = $"Score: \n{s}";            
        }

        gameplayScreen.SetActive(!b);
        gameOverScreen.SetActive(b);
    }

    public void TogglePauseScreen(bool b)
    {
        gameController.GetBlockerController.AllowMove = !b;
        pauseWindow.SetActive(b);

        if (!b)
        {
            gameController.GetSaveManager.AddToSave("", 0, 0);   
        }
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

    
}

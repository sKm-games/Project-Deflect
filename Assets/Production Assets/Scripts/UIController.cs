using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private LevelController levelController;
    [SerializeField] private GameObject mainMenuUI;
    private GameObject startScreen;
    [SerializeField] private GameObject gameUI;
    private GameObject gameplayScreen;
    private TextMeshProUGUI gameplayLevelText;
    private TextMeshProUGUI gameplayScoreText;
    private TextMeshProUGUI gameplayCountText;
    private GameObject gameOverScreen;
    private TextMeshProUGUI gameOverTitleText;
    private TextMeshProUGUI gameOverText;


    private void Awake()
    {
        startScreen = mainMenuUI.transform.GetChild(0).gameObject;

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
        gameplayScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        gameplayCountText.text = "0";
        UpdateScoreText(0);
        UpdateLevelText(0);
    }

    public void MainMenuToGame()
    {
        startScreen.SetActive(false);
        gameplayScreen.SetActive(true);
    }

    public void UpdateCountdownText(int a, bool b = true)
    {
        gameplayCountText.gameObject.SetActive(b);
        gameplayCountText.text = a.ToString();
    }

    public void UpdateScoreText(int s)
    {
        gameplayScoreText.text = $"Score: {s}";
    }

    public void UpdateLevelText(int l)
    {
        gameplayLevelText.text = $"Level: {l+1}/{levelController.GetLevelCount()}";
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
}

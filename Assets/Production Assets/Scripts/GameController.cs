using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private UIController uiController;
    [SerializeField] private LevelController levelController;
    private ProjectileController projectileController;
    private DifficultyController difficultyController;
    [SerializeField] private BlockerController blockerController;

    private bool gameIsRunning;
    public bool GameIsRunning
    {
        get
        {
            return gameIsRunning;
        }
    }

    [SerializeField] private int lifes;
    [SerializeField] private int score;

    private void Awake()
    {
        projectileController = levelController.GetComponent<ProjectileController>();
        difficultyController = levelController.GetComponent<DifficultyController>();
        blockerController.ToggleBlocker(false);    
        UpdateScore(0);

        levelController.Init();
    }

    public void StartGame(int d)
    {        
        if (!gameIsRunning)
        {
            Debug.Log("Start Game");
            score = 0;
            difficultyController.SetDifficutly(d);
            levelController.SetupLevel();
            gameIsRunning = true;            
            blockerController.Init(difficultyController.GetCurrentDifficulty);
            blockerController.ToggleBlocker(true);
        }
    }

    public void SetLifes(int l)
    {
        lifes = l;
    }

    public void UpdateLifes(int a)
    {
        if (DebugController.infiniteLife)
        {
            return;
        }

        lifes += a;

        if (lifes <= 0)
        {            
            GameIsOver();
        }
    }

    public void UpdateScore(int a)
    {
        score += a;
        uiController.UpdateScoreText(score);
    }

    private void GameIsOver()
    {
        Debug.Log("GameController: GameIsOver");
        gameIsRunning = false;
        blockerController.ToggleBlocker(false);
        uiController.ToggleGameOverScreen(true, false, score);
    }

    public void Winner()
    {
        Debug.Log("GameController: Winner");
        gameIsRunning = false;
        blockerController.ToggleBlocker(false);
        uiController.ToggleGameOverScreen(true, true, score);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

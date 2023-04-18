using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    [SerializeField] private UIController uiController;
    [SerializeField] private ScoreController scoreController;
    public ScoreController GetScoreController
    {
        get
        {
            return scoreController;
        }
    }
    [SerializeField] private LevelController levelController;
    public LevelController GetLevelController
    {
        get
        {
            return levelController;
        }
    }
    [SerializeField] private SaveManager saveManager;
    public SaveManager GetSaveManager
    {
        get
        {
            return saveManager;
        }
    }
    private ProjectileController projectileController;
    public ProjectileController GetProjectilesController
    {
        get
        {
            return projectileController;
        }
    }
    private DifficultyController difficultyController;
    public DifficultyController GetDifficultyController
    {
        get
        {
            return difficultyController;
        }
    }
    [SerializeField] private BlockerController blockerController;
    public BlockerController GetBlockerController
    {
        get
        {
            return blockerController;
        }
    }
    [SerializeField] private SoundController soundController;
    public SoundController GetSoundController
    {
        get
        {
            return soundController;
        }
    }    


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

    private void Awake()
    {
        projectileController = levelController.GetComponent<ProjectileController>();
        difficultyController = levelController.GetComponent<DifficultyController>();
        blockerController.ToggleBlocker(false);    
        
        levelController.Init();
    }

    private void Start()
    {
        saveManager.GetSaveInfo();
    }

    public void StartGame(int d)
    {        
        if (!gameIsRunning)
        {
            Debug.Log("Start Game");
            scoreController.ResetScore();
            difficultyController.SetDifficutly(d);
            levelController.SetupLevel(true);
            gameIsRunning = true;            
            blockerController.Init(difficultyController.GetCurrentDifficulty);
            uiController.UpdateLevelText(0, difficultyController.GetCurrentDifficulty.endLess);
            uiController.UpdateScoreText(0);
            blockerController.ToggleBlocker(true);
            Time.timeScale = 1;
        }
    }

    public void VideoContinue()
    {
        levelController.SetupLevel(false);
        uiController.ToggleGameOverScreen(false, false, 0);
        blockerController.ToggleBlocker(true);
        gameIsRunning = true;
    }

    public void RetryDifficulty()
    {
        StartGame(difficultyController.GetDifficultyIndex());
        uiController.ToggleGameOverScreen(false, false, 0);
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
            GameIsOver();
        }
    }

    private void GameIsOver()
    {
        Debug.Log("GameController: GameIsOver");
        gameIsRunning = false;
        blockerController.ToggleBlocker(false);
        uiController.ToggleGameOverScreen(true, false, scoreController.GetTotalScore);
        saveManager.AddToSave(difficultyController.GetCurrentDifficulty.ID, scoreController.GetTotalScore, scoreController.GetDeflections);        
    }

    public void Winner()
    {
        Debug.Log("GameController: Winner");
        gameIsRunning = false;
        blockerController.ToggleBlocker(false);
        uiController.ToggleGameOverScreen(true, true, scoreController.GetTotalScore);
        saveManager.AddToSave(difficultyController.GetCurrentDifficulty.ID, scoreController.GetTotalScore, scoreController.GetDeflections);
    }

    public void ReloadGame()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

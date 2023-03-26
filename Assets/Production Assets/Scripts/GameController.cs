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
    }

    [SerializeField] private int lifes;

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
            levelController.SetupLevel();
            gameIsRunning = true;            
            blockerController.Init(difficultyController.GetCurrentDifficulty);
            uiController.UpdateLevelText(0);
            uiController.UpdateScoreText(0);
            blockerController.ToggleBlocker(true);
        }
    }

    public void SetLifes(int l)
    {
        lifes = l;
    }

    public void UpdateLifes(int a)
    {
        if (DebugSystem.InfinitLife)
        {
            return;
        }

        lifes += a;

        if (lifes <= 0)
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

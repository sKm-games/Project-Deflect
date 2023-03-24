using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private int lifes, level, score;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI scoreText, levelText;    
    [SerializeField] private ProjectileController projectileController;
    [SerializeField] private BlockerController blockerController;    
    [SerializeField] private float shakeDuration;

    private bool gameIsRunning;
    private Vector3 camStartPos;

    private void Start()
    {
        camStartPos = Camera.main.transform.position;
        gameOverScreen.SetActive(false);
        UpdateScore(0);
    }

    public void StartGame()
    {
        if (!gameIsRunning)
        {
            Debug.Log("Start Game");
            gameIsRunning = true;
            projectileController.FindEnemies(level);            
            level++;
            levelText.text = "Level: " + level;
            blockerController.ToggleBlocker(true);
        }
    }

    public void SetLifes(int l)
    {
        lifes = l;
    }

    public void UpdateLifes(int a)
    {
        int templifes = lifes;
        lifes += a;

        if (lifes <= 0)
        {
            GameIsOver();
        }
    }

    public void UpdateScore(int a)
    {
        score += a;
        scoreText.text = "Score: " + score;
    }

    public void RoundOver()
    {
        if (gameIsRunning)
        {
            level++;
            levelText.text = "Level: " + level;
            projectileController.SpawnEnemy(level);

        }
    }

    void GameIsOver()
    {
        gameIsRunning = false;
        blockerController.ToggleBlocker(false);
        gameOverScreen.SetActive(true);
        TextMeshProUGUI t = gameOverScreen.GetComponentInChildren<TextMeshProUGUI>();
        t.text = "Game Over!\nTo much of the city is destroyed,\nthere is no stopping them now\n\nScore: " + score + "\n\nTry again?";
    }

    public void Winner()
    {
        gameIsRunning = false;
        blockerController.ToggleBlocker(false);
        gameOverScreen.SetActive(true);
        TextMeshProUGUI t = gameOverScreen.GetComponentInChildren<TextMeshProUGUI>();
        t.text = "Winner!\nLook like they are out of ammo.\nYou saved the town\n\nScore: " + score + "\n\nTry again?";
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

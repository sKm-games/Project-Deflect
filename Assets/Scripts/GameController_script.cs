using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController_script : MonoBehaviour
{

    [SerializeField] private int lifes, level, score;
    [SerializeField] private Transform buildingHolder;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI scoreText, levelText;
    private List<Building_script> buildings;
    [SerializeField] private EnemySpawner_script spawner;
    [SerializeField] private BlockerMovement_script blocker;
    bool gameIsRunning;
    [SerializeField] private float shakeDuration;
    private Vector3 camStartPos;   

    private void Start()
    {
        camStartPos = Camera.main.transform.position;

        buildings = new List<Building_script>(buildingHolder.GetComponentsInChildren<Building_script>());
        /*foreach (Building_script b in buildings)
        {
            b.gameObject.SetActive(false);
        }
        buildings[0].gameObject.SetActive(true);*/
        gameOverScreen.SetActive(false);
        SetLifes();
        UpdateScore(0);        
    }

    public void StartGame()
    {
        if (!gameIsRunning)
        {
            gameIsRunning = true;
            spawner.FindEnemies(level);
            //spawner.SpawnEnemy(level);
            level++;
            levelText.text = "Level: " + level;
            blocker.ToggleBlocker(true);          
        }
    }

    void SetLifes()
    {
        lifes = buildings.Count;
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
            spawner.SpawnEnemy(level);

        }        
    }

    void GameIsOver()
    {
        gameIsRunning = false;
        blocker.ToggleBlocker(false);
        gameOverScreen.SetActive(true);
        TextMeshProUGUI t = gameOverScreen.GetComponentInChildren<TextMeshProUGUI>();
        t.text = "Game Over!\nTo much of the city is destroyed,\nthere is no stopping them now\n\nScore: " + score + "\n\nTry again?";
    }

    public void Winner()
    {
        gameIsRunning = false;
        blocker.ToggleBlocker(false);
        gameOverScreen.SetActive(true);
        TextMeshProUGUI t = gameOverScreen.GetComponentInChildren<TextMeshProUGUI>();
        t.text = "Winner!\nLook like they are out of ammo.\nYou saved the town\n\nScore: " + score + "\n\nTry again?";
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}

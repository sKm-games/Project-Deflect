using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private UIController uiController;
    private DifficultyController difficultyController;
    private ProjectileController projectileController;

    [SerializeField] private int startTimer;
    [SerializeField] private int level;
    public int GetLevel
    {
        get
        {
            return level;
        }
    }
    
    [SerializeField] private int totalShoots;

    [SerializeField] private Transform spawnPointHolder;
    private List<SpawnpointObject> spawnPointList;

    [SerializeField] private Transform targetHolder;
    private List<TargetObject> targetsList;
    [SerializeField] private List<TargetObject> activeTargets;

    [SerializeField] private List<int> levelList;

    private void Awake()
    {
        difficultyController = this.GetComponent<DifficultyController>();
        projectileController = this.GetComponent<ProjectileController>();
    }

    public void Init()
    {
        targetsList = new List<TargetObject>(targetHolder.GetComponentsInChildren<TargetObject>());
        spawnPointList = new List<SpawnpointObject>(spawnPointHolder.GetComponentsInChildren<SpawnpointObject>());
        
        foreach (TargetObject t in targetsList)
        {
            t.Init();            
        }
    }

    public void SetupLevel()
    {
        totalShoots = 0;
        level = 0;
        levelList = new List<int>(difficultyController.GetCurrentDifficulty.LevelsProjectiles);
        SetTargets();        
        StartCoroutine(IECountDown());
        
        //StartLevel();
    }

    IEnumerator IECountDown()
    {
        int a = startTimer;
        
        while (a >= 0)
        {
            uiController.UpdateCountdownText(a);
            a--;
            yield return new WaitForSeconds(1f);
        }
        uiController.UpdateCountdownText(0, false);
        StartLevel();
    }

    public void StartLevel()
    {       
        if (level >= levelList.Count)
        {
            gameController.Winner();
            return;
        }

        uiController.UpdateLevelText(level);

        int amount = levelList[level];
        StartCoroutine(IEProgressLevel(amount));
    }    

    IEnumerator IEProgressLevel(int a)
    {
        totalShoots = 0;
        for (int i = 0; i < a; i++)
        {
            SpawnpointObject sp = GetRandomSpawn();
            projectileController.DoLaunchProjectile(sp);
            totalShoots++;

            yield return new WaitForSeconds(difficultyController.GetCurrentDifficulty.NextShotDelay);
        }

        while (totalShoots < a)
        {
            yield return null;
        }
        yield return new WaitForSeconds(difficultyController.GetCurrentDifficulty.NextLevelDelay);

        LevelOver();
    }

    public void LevelOver()
    {
        if (gameController.GameIsRunning)
        {
            level++;            
            StartLevel();
        }
    }

    private void SetTargets()
    {
        activeTargets = new List<TargetObject>();
        int t = difficultyController.GetCurrentDifficulty.TargetAmount;
        for (int i = 0; i < targetsList.Count; i++)
        {
            if (i < t)
            {
                targetsList[i].gameObject.SetActive(true);
                activeTargets.Add(targetsList[i]);
            }
            else
            {
                targetsList[i].gameObject.SetActive(false);
            }            
        }

        gameController.SetLifes(activeTargets.Count);
    }

    public Transform GetTarget()
    {
        activeTargets.Shuffle();

        if (activeTargets[0].IsDead)
        {
            activeTargets.RemoveAt(0);
        }

        return activeTargets[0].transform;            
    }

    public SpawnpointObject GetRandomSpawn()
    {
        spawnPointList.Shuffle();

        foreach (SpawnpointObject s in spawnPointList)
        {
            if (s.IsReady)
            {
                return s;
            }
        }

        return spawnPointList[0];
    }

    public int GetLevelCount()
    {
        return difficultyController.GetCurrentDifficulty.LevelsProjectiles.Count;
    }

}

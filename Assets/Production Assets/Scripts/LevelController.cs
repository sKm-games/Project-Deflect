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
    [SerializeField] private List<SpawnpointObject> spawnPointList;

    [SerializeField] private Transform targetHolder;
    [SerializeField] private List<TargetObject> targetsPoolList;
    [SerializeField] private List<TargetObject> activeTargetsPoolList;

    [SerializeField] private List<int> levelList;

    private void Awake()
    {
        difficultyController = this.GetComponent<DifficultyController>();
        projectileController = this.GetComponent<ProjectileController>();
    }

    public void Init()
    {
        targetsPoolList = new List<TargetObject>(targetHolder.GetComponentsInChildren<TargetObject>());
        spawnPointList = new List<SpawnpointObject>(spawnPointHolder.GetComponentsInChildren<SpawnpointObject>());
        
        foreach (TargetObject t in targetsPoolList)
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

        /*float delay = difficultyController.GetNextLevelDelay(level);
        yield return new WaitForSeconds(delay);

        //yield return new WaitForSeconds(difficultyController.GetCurrentDifficulty.NextLevelDelay);

        LevelOver();*/
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
        activeTargetsPoolList = new List<TargetObject>();
        int t = difficultyController.GetCurrentDifficulty.TargetAmount;
        for (int i = 0; i < targetsPoolList.Count; i++)
        {
            if (i < t)
            {
                targetsPoolList[i].gameObject.SetActive(true);
                activeTargetsPoolList.Add(targetsPoolList[i]);
            }
            else
            {
                targetsPoolList[i].gameObject.SetActive(false);
            }            
        }

        gameController.SetLifes(activeTargetsPoolList.Count);
    }

    public Transform GetTarget()
    {
        activeTargetsPoolList.Shuffle();

        if (activeTargetsPoolList[0].IsDead)
        {
            activeTargetsPoolList.RemoveAt(0);
        }

        return activeTargetsPoolList[0].transform;            
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

    public void GetLevelInfo(out int l, out string d)
    {
        l = difficultyController.GetCurrentDifficulty.LevelsProjectiles.Count;
        d = difficultyController.GetCurrentDifficulty.Name;
    }

}

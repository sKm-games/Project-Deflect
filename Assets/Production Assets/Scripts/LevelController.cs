using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private UIController uiController;
    [SerializeField] private GooglePlayController googlePlayController;
    private DifficultyController difficultyController;
    private ProjectileController projectileController;

    [SerializeField] private int startTimer;
    [SerializeField] private int levelRef; //used to as index for levelList    
    [SerializeField] private int levelCount; //used to track level for ui, mostly for endless mode
    
    [SerializeField] private int totalShoots;

    [SerializeField] private Transform spawnPointHolder;
    [SerializeField] private List<SpawnpointObject> spawnPointList;
    private SpawnpointObject currentSpawnpoint;

    [SerializeField] private Transform targetHolder;
    [SerializeField] private List<TargetObject> targetsPoolList;
    [SerializeField] private List<TargetObject> activeTargetsPoolList;
    private Transform previousTarget;

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

    public void SetupLevel(bool newRun)
    {
        if (newRun)
        {
            totalShoots = 0;
            levelRef = 0;
            levelCount = 0;
            levelList = new List<int>(difficultyController.GetCurrentDifficulty.LevelsProjectiles);
        }        
        SetTargets();        
        StartCoroutine(IECountDown());
    }

    IEnumerator IECountDown()
    {
        gameController.GetSoundController.PlaySFX("alarm");
        int a = startTimer;
        
        while (a >= 0)
        {
            uiController.UpdateCountdownText(a);
            a--;
            yield return new WaitForSeconds(1f);
        }
        uiController.UpdateCountdownText(0, false);
        StartLevel();
        
        yield return new WaitForSeconds(1f);        
        gameController.GetSoundController.StopLoopSFX("alarm");
    }

    public void StopProgress()
    {
        StopAllCoroutines();
        projectileController.StopProgress();

        foreach (SpawnpointObject sp in spawnPointList)
        {
            sp.StopProgress();
        }

        uiController.UpdateCountdownText(0, false);
    }

    public void StartLevel()
    {       
        if (levelRef >= levelList.Count)
        {
            if (difficultyController.GetCurrentDifficulty.endLess) //reset level count and shuffle list for endless
            {
                levelList.Shuffle();
                levelRef = 0;
            }
            else
            {
                gameController.Winner();
                return;
            }            
        }

        uiController.UpdateLevelText(levelCount, difficultyController.GetCurrentDifficulty.endLess);

        int amount = levelList[levelRef];
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
        
        while (!gameController.GameIsRunning)
        {
            yield return null;
        }
        
        while (totalShoots < a)
        {   
            yield return null;
        }
    }

    public void LevelOver()
    {
        if (gameController.GameIsRunning)
        {
            levelRef++;
            levelCount++;
            StartLevel();
            googlePlayController.GetAchievementController.CheckLevelAchievements(difficultyController.GetCurrentDifficulty.Name, levelCount);
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
                targetsPoolList[i].ResetTagets();
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

        int tries = 0;
        while (activeTargetsPoolList[0].transform == previousTarget && tries < 6)
        {
            activeTargetsPoolList.Shuffle();
            tries++;
        }

        Transform t = activeTargetsPoolList[0].transform;

        foreach (TargetObject to in activeTargetsPoolList)
        {
            if (currentSpawnpoint.CheckValidTarget(to))
            {
                t = to.transform;
                break;
            }
        }
        previousTarget = t;
        return t;
    }

    /*public Transform GetTarget() //Old
    {
        activeTargetsPoolList.Shuffle();

        if (activeTargetsPoolList[0].IsDead)
        {
            activeTargetsPoolList.RemoveAt(0);
        }

        return activeTargetsPoolList.Count == 0 ? null : activeTargetsPoolList[0].transform;
    }*/

    public SpawnpointObject GetRandomSpawn()
    {
        spawnPointList.Shuffle();

        currentSpawnpoint = spawnPointList[0];

        foreach (SpawnpointObject s in spawnPointList)
        {
            if (s.IsReady)
            {
                currentSpawnpoint = s;
                break;
                //return s;
            }
        }

        //return spawnPointList[0];
        return currentSpawnpoint;
        
    }

    public void GetLevelInfo(out int l, out string d)
    {
        l = difficultyController.GetCurrentDifficulty.LevelsProjectiles.Count;
        d = difficultyController.GetCurrentDifficulty.Name;
    }

    public void ResetTargets()
    {
        foreach (TargetObject t in targetsPoolList)
        {
            t.ResetTagets();
        }

        gameController.SetLifes(activeTargetsPoolList.Count);
    }

}

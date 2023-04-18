using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private UIController uiController;
    [SerializeField] private SoundController soundController;
    [SerializeField] private List<DifficultyDataClass> difficulties;
    [SerializeField] private DifficultyDataClass currentDifficulty;
    public DifficultyDataClass GetCurrentDifficulty
    {
        get
        {
            return currentDifficulty;
        }
    }    

    public void SetDifficutly(int i)
    {
        currentDifficulty = difficulties[i];
    }

    public float GetNextLevelDelay(int l)
    {
        return currentDifficulty.NextLevelDelay * currentDifficulty.LevelsProjectiles[l];
    }

    public int GetDifficultyIndex()
    {
        return difficulties.FindIndex(x => x == currentDifficulty);        
    }

    public string GetDifficultyLeaderboardRef(int i)
    {
        Debug.Log($"GetDifficultyLeaderboardRef using index: {i}");
        return difficulties[i].LeaderboardID;
    }
}

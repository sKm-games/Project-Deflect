using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
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
}

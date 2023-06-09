using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyDataObject", menuName = "ScriptableObjects/Difficulties", order = 1)]
public class DifficultyDataClass : ScriptableObject
{
    [Header("Basic Info")]
    public string ID; //Unuqie id
    public string Name; //Name shown on UI, etc
    public string LeaderboardID; //Leaderboard ID from Google Play
    public ModeEnums Mode; //Made identifier, used by Google Play

    [Header("Timing Info")]
    public float AimTime; //How long it aims, befor shooting
    public float ShotTime; //Warning delay befor shooting
    public float NextShotDelay; //Delay between shoots
    public float NextLevelDelay; //Delay between levels
    
    [Header("Laser Info")]
    public float AimLaserSize; //Size of the laser while aiming
    public Color AimLaserColor; //Color of the laser while aiming
    public float ShotLaserSize; //Size of the laser before shooting
    public Color ShotLaserColor; //Color of the laser before shooting    

    [Header("Target Info")]
    [Range(1,4)]
    public int TargetAmount = 4; //Number of targets it show have/lifes    

    [Header("Blocker Info")]
    public Vector2 BlockerScale; //the size of the blocker
    public Color BlockerColor; //color of the blocker, temp might change to sprite later

    [Header("Projectiles Info")]
    public float ProjectileSpeed; //How fast the projectiles will be launced

    [Header("Levels Info")]
    public List<int> LevelsProjectiles; //List of the projectiles per level
}

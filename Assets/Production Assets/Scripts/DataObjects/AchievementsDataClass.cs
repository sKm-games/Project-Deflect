using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AchievementsDataObject", menuName = "ScriptableObjects/Achievements", order = 1)]
public class AchievementsDataClass : ScriptableObject
{
    [Header("GooglePlay Info")]    
    public string AchievementID; //id from Google Play Console
    public int IncrementValue; //How much the achievement should increase per event
    public bool Instant;  //If the achievement is instant
    
    [Header("Trigger Info")]
    public string NameRef; //Used to find the achievement object
    public int Target; //Used for certain achievements to trigger when target is reached
    public ValueCompareEnums TargetComparing; //Used to know if how the target value should be handles when comparing
    public ModeEnums Mode; ////Used for certain achievements to trigger on specific modes
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AchievementsDataObject", menuName = "ScriptableObjects/Achievements", order = 1)]
public class AchievementsDataClass : ScriptableObject
{
    public string NameRef; //name to find
    public string AchievementID; //id from Google Play Console
    public int IncrementValue; //How much the achievement should increase per event
    public bool Instant;  //If the achievement is instant
}

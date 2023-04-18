﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;


public class AchievementController : MonoBehaviour
{    
    [SerializeField] private SoundController soundController;
    [SerializeField] private SaveManager saveManager;

    [SerializeField] private List<AchievementsDataClass> achievementsDataList;
 
    [SerializeField] private Button achievButton;
    public Button AchievButton
    {
        get
        {
            return achievButton;
        }
    }

    [SerializeField] bool doDebug;

    private void Awake()
    {
#if UNITY_ANDROID
        achievButton.interactable = false;
        PlayGamesPlatform.Activate();
#endif
    }

    public void ShowAchievScreen()
    {
#if UNITY_ANDROID
        soundController.PlaySFX("Button");
        if (!PlayGamesPlatform.Instance.IsAuthenticated()) //not logged in skip
        {
            DebugSystem.UpdateDebugText("Google Play not looged in");
            return;
        }
        Social.ShowAchievementsUI();
#endif
    }

    public void CheckLevelAchievements(string modeRef, int l)
    {
        DebugSystem.UpdateDebugText($"Mode Ref: {modeRef}, level {l}", false, doDebug);
        if (modeRef == "Endless" && l > 9)
        {
            CheckAchievement("TestKeepGoing");
        }
    }

    public void CheckDeflectedAchievements()
    {
        CheckAchievement("TestDeflect5");
        CheckAchievement("TestDeflect20");
    }

    public void CheckHealthAchievements()
    {
        CheckAchievement("TestBurning10");
    }

    public void CheckAchievement(string nameRef)
    {
#if UNITY_ANDROID
        AchievementsDataClass info = achievementsDataList.Find((x) => x.NameRef == nameRef);

        if (info == null)
        {
            DebugSystem.UpdateDebugText($"AchievementController: CheckAchievement: Invalid nameRef {nameRef}", true, doDebug);
            return;
        }
        int i = info.Instant == true ? 100 : info.IncrementValue;

        if (!PlayGamesPlatform.Instance.IsAuthenticated()) //not logged in skip
        {
            DebugSystem.UpdateDebugText($"AchievementController: CheckAchievement: Not logged in", false, doDebug);
            return;
        }

        PlayGamesPlatform.Instance.IncrementAchievement(info.AchievementID, i, (bool success) => { });
#endif
    }

}
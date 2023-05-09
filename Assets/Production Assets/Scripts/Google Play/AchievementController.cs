using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;


public class AchievementController : MonoBehaviour
{
    [SerializeField]
    private AchievementsDataClass easyModeDone;
    [SerializeField]
    private AchievementsDataClass mediumModeDone;
    [SerializeField]
    private AchievementsDataClass hardModeDone;
    [SerializeField]
    private AchievementsDataClass impossibleModeDone;
    [SerializeField]
    private List<AchievementsDataClass> modeLevelsDataList;
    [SerializeField]
    private List<AchievementsDataClass> deflectionsDataList;
    [SerializeField]
    private List<AchievementsDataClass> targetsDataList;

    [SerializeField]
    private List<AchievementsDataClass> othersDataList;


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
        if (!GooglePlayController.CheckLogin()) //not logged in skip
        {
            DebugSystem.UpdateDebugText("Google Play not looged in");
            return;
        }
        Social.ShowAchievementsUI();
#endif
    }

    public void CheckLevelAchievements(int l)
    {
        if (!GooglePlayController.CheckLogin()) //not logged in skip
        {
            DebugSystem.UpdateDebugText($"AchievementController: CheckLevelAchievements: Not logged in", false, doDebug);
            return;
        }
        ModeEnums mode = ReferencesController.GetDifficultyController.GetCurrentDifficulty.Mode;

        foreach (AchievementsDataClass ad in modeLevelsDataList)
        {
            if (ad.Target >= l && mode == ad.Mode)
            {
                CheckAchievement(ad);
            }
        }
    }

    public void CheckModeDoneAchievements()
    {
        if (!GooglePlayController.CheckLogin()) //not logged in skip
        {
            DebugSystem.UpdateDebugText($"AchievementController: CheckModeDoneAchievements: Not logged in", false, doDebug);
            return;
        }
        ModeEnums mode = ReferencesController.GetDifficultyController.GetCurrentDifficulty.Mode;

        switch (mode)
        {
            case ModeEnums.Easy:
                CheckAchievement(easyModeDone);
                break;
            case ModeEnums.Medium:
                CheckAchievement(mediumModeDone);
                break;
            case ModeEnums.Hard:
                CheckAchievement(hardModeDone);
                break;
            case ModeEnums.Impossible:
                CheckAchievement(impossibleModeDone);
                break;
            case ModeEnums.Endless:
            default:
                break;
        }
    }

    public void CheckDeflectedAchievements()
    {
        if (!GooglePlayController.CheckLogin()) //not logged in skip
        {
            DebugSystem.UpdateDebugText($"AchievementController: CheckAchievement: Not logged in", false, doDebug);
            return;
        }
        foreach (AchievementsDataClass ad in deflectionsDataList)
        {
            CheckAchievement(ad);
        }
    }

    public void CheckTargetAchievements()
    {
        if (!GooglePlayController.CheckLogin()) //not logged in skip
        {
            DebugSystem.UpdateDebugText($"AchievementController: CheckAchievement: Not logged in", false, doDebug);
            return;
        }
        foreach (AchievementsDataClass ad in targetsDataList)
        {
            CheckAchievement(ad);
        }
    }

    public void CheckOtherAchievements(string id, int target = 0)
    {
        if (!GooglePlayController.CheckLogin()) //not logged in skip
        {
            DebugSystem.UpdateDebugText($"AchievementController: CheckOtherAchievements: Not logged in", false, doDebug);
            return;
        }

        foreach (AchievementsDataClass ad in othersDataList)
        {
            if (ad.NameRef == id && ad.Target >= target)
            {
                CheckAchievement(ad);
                return;
            }                
        }
    }

    private void CheckAchievement(AchievementsDataClass ad)
    {
#if UNITY_ANDROID
        DebugSystem.UpdateDebugText($"AchievementController: CheckAchievement with nameRef {ad.NameRef}", false, doDebug);

        if (ad == null)
        {
            DebugSystem.UpdateDebugText($"AchievementController: CheckAchievement: Invalid nameRef {ad.NameRef}", true, doDebug);
            return;
        }

        if (ad.Instant)
        {
            Social.ReportProgress(ad.AchievementID, 100.00, (bool success) => 
            {
                if (!success)
                {
                    DebugSystem.UpdateDebugText($"AchievementController: CheckAcievement: Failed to report instant using nameRef {ad.NameRef}, check info");
                }
            });
        }
        else
        {
            PlayGamesPlatform.Instance.IncrementAchievement(ad.AchievementID, ad.IncrementValue, (bool success) =>
            {
                if (!success)
                {
                    DebugSystem.UpdateDebugText($"AchievementController: CheckAcievement: Failed to report incremental using nameRef {ad.NameRef}, check info");
                }
            });
        }        
#endif
    }

}

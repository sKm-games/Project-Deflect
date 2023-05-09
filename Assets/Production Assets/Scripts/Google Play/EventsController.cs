using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;

public class EventsController : MonoBehaviour
{
    [SerializeField]
    private EventInfoClass easyMode;
    [SerializeField]
    private EventInfoClass mediumMode;
    [SerializeField]
    private EventInfoClass hardMode;
    [SerializeField]
    private EventInfoClass impossibleMode;
    [SerializeField]
    private List<EndlessEventInfoClass> endlessMode;
    [SerializeField]
    private string deflectionsID;
    [SerializeField]
    private string targetsDestroydID;
    [SerializeField]
    private string projectilesLaunchedID;

    public void ModeStartedEvent()
    {
        if (!GooglePlayController.CheckLogin())
        {
            return;
        }

        ModeEnums mode = ReferencesController.GetDifficultyController.GetCurrentDifficulty.Mode;
        string id;

        switch (mode)
        {
            case ModeEnums.Easy:
                id = easyMode.StartedID;
                break;
            case ModeEnums.Medium:
                id = mediumMode.StartedID;
                break;
            case ModeEnums.Hard:
                id = hardMode.StartedID;
                break;
            case ModeEnums.Impossible:
                id = impossibleMode.StartedID;
                break;
            case ModeEnums.Endless:
                //Skip
                return;
            default:
                DebugSystem.UpdateDebugText($"EventsController: LevelEvent: Invalid Mode used {mode}");
                return;
        }

        PlayGamesPlatform.Instance.Events.IncrementEvent(id, 1);
    }

    public void ModeCompletedEvent()
    {
        if (!GooglePlayController.CheckLogin())
        {
            return;
        }
        ModeEnums mode = ReferencesController.GetDifficultyController.GetCurrentDifficulty.Mode;
        string id;
        switch (mode)
        {
            case ModeEnums.Easy:
                id = easyMode.CompletedID;
                break;
            case ModeEnums.Medium:
                id = mediumMode.CompletedID;
                break;
            case ModeEnums.Hard:
                id = hardMode.CompletedID;
                break;
            case ModeEnums.Impossible:
                id = impossibleMode.CompletedID;
                break;
            case ModeEnums.Endless:
                //Skip
                return;
            default:
                DebugSystem.UpdateDebugText($"EventsController: ModeCompletedEvent: Invalid Mode used {mode}");
                return;
        }

        PlayGamesPlatform.Instance.Events.IncrementEvent(id, 1);
    }

    public void ModeGameOverEvent()
    {
        if (!GooglePlayController.CheckLogin())
        {
            return;
        }
        ModeEnums mode = ReferencesController.GetDifficultyController.GetCurrentDifficulty.Mode;
        string id;
        switch (mode)
        {
            case ModeEnums.Easy:
                id = easyMode.GameOverID;
                break;
            case ModeEnums.Medium:
                id = mediumMode.GameOverID;
                break;
            case ModeEnums.Hard:
                id = hardMode.GameOverID;
                break;
            case ModeEnums.Impossible:
                id = impossibleMode.GameOverID;
                break;
            case ModeEnums.Endless:
                //Skip
                return;
            default:
                DebugSystem.UpdateDebugText($"EventsController: ModeGameOverEvent: Invalid Mode used {mode}");
                return;
        }

        PlayGamesPlatform.Instance.Events.IncrementEvent(id, 1);
    }


    public void ModeQuitEvent()
    {
        if (!GooglePlayController.CheckLogin())
        {
            return;
        }
        ModeEnums mode = ReferencesController.GetDifficultyController.GetCurrentDifficulty.Mode;
        string id;
        switch (mode)
        {
            case ModeEnums.Easy:
                id = easyMode.QuitID;
                break;
            case ModeEnums.Medium:
                id = mediumMode.QuitID;
                break;
            case ModeEnums.Hard:
                id = hardMode.QuitID;
                break;
            case ModeEnums.Impossible:
                id = impossibleMode.QuitID;
                break;
            case ModeEnums.Endless:
                //Skip
                return;
            default:
                DebugSystem.UpdateDebugText($"EventsController: ModeQuitEvent: Invalid Mode used {mode}");
                return;
        }

        PlayGamesPlatform.Instance.Events.IncrementEvent(id, 1);
    }

    public void RebuildEvent()
    {
        if (!GooglePlayController.CheckLogin())
        {
            return;
        }
        ModeEnums mode = ReferencesController.GetDifficultyController.GetCurrentDifficulty.Mode;
        string id;
        switch (mode)
        {
            case ModeEnums.Easy:
                id = easyMode.RebuildID;
                break;
            case ModeEnums.Medium:
                id = mediumMode.RebuildID;
                break;
            case ModeEnums.Hard:
                id = hardMode.RebuildID;
                break;
            case ModeEnums.Impossible:
                id = impossibleMode.RebuildID;
                break;
            case ModeEnums.Endless:
                //Skip
                return;
            default:
                DebugSystem.UpdateDebugText($"EventsController: RebuildEvent: Invalid Mode used {mode}");
                return;
        }

        PlayGamesPlatform.Instance.Events.IncrementEvent(id, 1);
    }

    public void DeflectionEvent(int d)
    {
        if (!GooglePlayController.CheckLogin())
        {
            return;
        }
        PlayGamesPlatform.Instance.Events.IncrementEvent(deflectionsID, (uint)d);
    }

    public void TargetsDestroyedEvent(int t)
    {
        if (!GooglePlayController.CheckLogin())
        {
            return;
        }
        PlayGamesPlatform.Instance.Events.IncrementEvent(targetsDestroydID, (uint)t);
    }

    public void ProjectilesLaunchedEvent(int p)
    {
        if (!GooglePlayController.CheckLogin())
        {
            return;
        }
        PlayGamesPlatform.Instance.Events.IncrementEvent(projectilesLaunchedID, (uint)p);
    }

    public void CheckEndlessLevelEvent(int l)
    {

        if (ReferencesController.GetDifficultyController.GetCurrentDifficulty.Mode != ModeEnums.Endless || !GooglePlayController.CheckLogin())
        {
            return;
        }
        foreach (EndlessEventInfoClass e in endlessMode)
        {
            if (e.LevelTrigger == l)
            {
                PlayGamesPlatform.Instance.Events.IncrementEvent(e.EventID, 1);
                break;
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Events;

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

    [SerializeField]
    private bool doDebug;

    private void Awake()
    {
        PlayGamesPlatform.Activate();
    }

    public void ModeStartedEvent()
    {
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
        
        DebugSystem.UpdateDebugText($"EventsController: Do ModeStartedEvent with ID {id}",false, doDebug);
        SendEventInfo(id, 1);
    }

    public void ModeCompletedEvent()
    {

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
        
        DebugSystem.UpdateDebugText($"EventsController: Do ModeCompletedEvent with ID {id}", false, doDebug);
        SendEventInfo(id, 1);
    }

    public void ModeGameOverEvent()
    {
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

        DebugSystem.UpdateDebugText($"EventsController: Do ModeGameOverEvent with ID {id}", false, doDebug);
        SendEventInfo(id, 1);
    }


    public void ModeQuitEvent()
    {
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
        
        SendEventInfo(id, 1);
        DebugSystem.UpdateDebugText($"EventsController: Do ModeQuitEvent with ID {id}", false, doDebug);
    }

    public void RebuildEvent()
    {
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

        SendEventInfo(id, 1);
        DebugSystem.UpdateDebugText($"EventsController: Do RebuildEvent with ID {id}", false, doDebug);
    }

    public void DeflectionEvent(int d)
    {
        SendEventInfo(deflectionsID, d);
        DebugSystem.UpdateDebugText($"EventsController: Do DeflectionEvent with ID {deflectionsID}", false, doDebug);
    }

    public void TargetsDestroyedEvent(int t)
    {
        SendEventInfo(targetsDestroydID, t);
        DebugSystem.UpdateDebugText($"EventsController: Do TargetsDestroyedEvent with ID {targetsDestroydID}", false, doDebug);
    }

    public void ProjectilesLaunchedEvent(int p)
    {
        SendEventInfo(projectilesLaunchedID, p);
        DebugSystem.UpdateDebugText($"EventsController: Do ProjectilesLaunchedEvent with ID {projectilesLaunchedID}", false, doDebug);
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
                SendEventInfo(e.EventID, l);
                DebugSystem.UpdateDebugText($"EventsController: Do CheckEndlessLevelEvent with ID {e.EventID}", false, doDebug);
                break;
            }
        }
    }

    private void SendEventInfo(string id, int a)
    {
        if (!GooglePlayController.CheckLogin())
        {
            DebugSystem.UpdateDebugText("EventsController: SendEventInfo: Skip, not logged in");
            return;
        }
        PlayGamesPlatform.Instance.Events.IncrementEvent(id, (uint)a);
        DebugSystem.UpdateDebugText($"EventsController: SendEventInfo with ID {id}");
    }
}


﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveManager : MonoBehaviour
{    
    [SerializeField] private GameController gameController;
    [SerializeField] private UIController uiController;
    [SerializeField] private GooglePlayController googlePlayController;
    
    [SerializeField] private int currentSaveVersion;
    [Serializable]
    public class DifficultySaveDataClass
    {
        public string DifficultyID;
        public int HighScore;
        public int Deflections;
    }

    [Serializable]
    public class SaveInfoClass
    {
        public int SaveVersion; //holds the SaveSystem versio, used to auto delet save files if needed
        //public int SFXMute; //holds the status of SFXmute
        //public int MusicMute; //holds the status of Musicmute
        public float SFXVolume;
        public float MusicVolume;
        public float Sensetivity; //blocker movement sens
        public int CurrentLevel; //holds current level ref
        public int LoggedInStatus; //0 not logged in, 1 logged in        
        public string LastPlayed; //Last time player played, used to give more hints
        public List<DifficultySaveDataClass> DifficultySaveDataList; //holds the info for each difficulty
    }

    [SerializeField] private SaveInfoClass mainSaveInfo;

    public SaveInfoClass MainSaveInfo
    {
        get
        {
            return mainSaveInfo;
        }
    }


    [SerializeField] private bool doDebug;
    
    public void AddToSave(string diffID, int score, int deflections)
    {        
        bool newEntry = true;
        SaveInfoClass tempSave = new SaveInfoClass();
        tempSave.DifficultySaveDataList = new List<DifficultySaveDataClass>();        
        if (mainSaveInfo != null)
        {
            tempSave = mainSaveInfo;
        }

        tempSave.SaveVersion = currentSaveVersion;
        gameController.GetSoundController.GetSaveInfo(out float s, out float m);

        if (string.IsNullOrEmpty(diffID)) //limited save
        {
            DebugSystem.UpdateDebugText("SaveManger: LimitedSave", false, doDebug);          
            tempSave.LastPlayed = DateTime.Today.ToShortDateString();

            tempSave.LoggedInStatus = GooglePlayController.CheckLogin();            

            tempSave.SFXVolume = s;
            tempSave.MusicVolume = m;
            tempSave.Sensetivity = gameController.GetBlockerController.Sensetivity;
            SaveSystem.SaveData(tempSave);
            return;
        }

        foreach (DifficultySaveDataClass ds in mainSaveInfo.DifficultySaveDataList)
        {
            if (ds.DifficultyID == diffID)
            {
                if (ds.HighScore < score)
                {
                    ds.HighScore = score;
                }

                ds.Deflections += deflections;
                newEntry = false;
                break;
            }
        }

        if (newEntry)
        {            
            DifficultySaveDataClass diffInfo = new DifficultySaveDataClass();
            diffInfo.DifficultyID = diffID;
            diffInfo.HighScore = score;
            tempSave.DifficultySaveDataList.Add(diffInfo);            
        }
        
        tempSave.SFXVolume = s;
        tempSave.MusicVolume = m;

        tempSave.LastPlayed = DateTime.Today.ToShortDateString();

        tempSave.LoggedInStatus = GooglePlayController.CheckLogin();
        SaveSystem.SaveData(tempSave);
        
        DebugSystem.UpdateDebugText("Saving Done", false, doDebug);        
    }
  
    public void GetSaveInfo()
    {
        googlePlayController.InitGooglePlay();

        if (SaveSystem.FileExist())
        {            
            mainSaveInfo = new SaveInfoClass();
            mainSaveInfo = SaveSystem.LoadBoards();            
            if (MainSaveInfo.SaveVersion != currentSaveVersion)
            {         
                string s = "New save system, deleting old save, Sorry\n";
                uiController.GetLoadingScreenController.UpdateText(s);
                SaveSystem.DeleteSaveFiles();
                gameController.ReloadGame();
                return;
            }
            gameController.GetSoundController.SetSaveInfo(mainSaveInfo.SFXVolume, mainSaveInfo.MusicVolume);
            gameController.GetBlockerController.SetSaveInfo(mainSaveInfo.Sensetivity);

            if (mainSaveInfo.LoggedInStatus == 1) //auto log in
            {                
                googlePlayController.LogIn();
            }          
        }
        else
        {            
            DebugSystem.UpdateDebugText("No save file, using default values", false, doDebug);
#if UNITY_ANDROID
            googlePlayController.FirstLogin();
#endif
            gameController.GetSoundController.SetSaveInfo(0.25f, 0.25f);
            gameController.GetBlockerController.SetSaveInfo(1f);
            AddToSave("", 0, 0); //Make new save file with default values
        }        
        Invoke("LoadingScreenOff", 0.5f);
    }

    private void LoadingScreenOff()
    {
        uiController.ToggleLoadingScreen(false);
        gameController.GetSoundController.PlayMusic();
    }

    public int GetScoreByID(string id)
    {        
        foreach (DifficultySaveDataClass info in mainSaveInfo.DifficultySaveDataList)
        {
            if (info.DifficultyID == id)
            {
                return info.HighScore;
            }
        }
        return 0;
    }

    public void NewSaveIDTesting()
    {
        currentSaveVersion = 99;
        GetSaveInfo();
    }
}

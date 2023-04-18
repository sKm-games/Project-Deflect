﻿using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.UI;

public class GooglePlayController : MonoBehaviour
{
    [SerializeField] private AchievementController achievementController;
    public AchievementController GetAchievementController
    {
        get
        {
            return achievementController;
        }
    }
    [SerializeField] private LeaderboardController leaderboardController;
    public LeaderboardController GetLeaderbordController
    {
        get
        {
            return leaderboardController;
        }
    }
    [SerializeField] private SoundController soundController;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private UIController uiController;

    [SerializeField] private Button loginButton;
    private Image loginImage;
    [SerializeField] private Sprite googlePlayLogin;
    [SerializeField] private Sprite googlePlayLogout;    

    [SerializeField] private bool doDebug;
    static private bool staticDoDebug;

    private void Awake()
    {
        loginImage = loginButton.GetComponent<Image>();
        staticDoDebug = doDebug;
    }

    public void InitGooglePlay()
    {
        DebugSystem.UpdateDebugText("Google Play LogIn, Init Google Play", false, doDebug);
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        DebugSystem.UpdateDebugText("Google Play LogIn, Init Google Play Done", false, doDebug);
    }

    public void FirstLogin()
    {
#if UNITY_ANDROID
        DebugSystem.UpdateDebugText("Google Play LogIn, Start first login, 0", false, doDebug);
        Social.localUser.Authenticate(success => {
            if (success)
            {
                DebugSystem.UpdateDebugText("Authentication successful");
                string userInfo = "Username: " + Social.localUser.userName +
                    "\nUser ID: " + Social.localUser.id +
                    "\nIsUnderage: " + Social.localUser.underage;
                DebugSystem.UpdateDebugText(userInfo, false, doDebug);
                UpdateLogInButton();
                CancelInvoke("LoginError");
            }
            else
            {                
                DebugSystem.UpdateDebugText($"Authentication failed", false, true);
                Invoke("LoginError", 5f);
            }            
        });
#endif
    }

    private void LoginError()
    {
        uiController.ToggleGooglePlayErrorScreen(true);
    }

    public void LogIn()
    {
        DebugSystem.UpdateDebugText("Google Play LogIn, Start normal login, 0", false, doDebug);
#if UNITY_ANDROID
        DebugSystem.UpdateDebugText("Google Play LogIn, Start login status " + Social.localUser.authenticated, false, doDebug);
        soundController.PlaySFX("Button");
        Social.localUser.Authenticate(success => {
            if (success)
            {
                DebugSystem.UpdateDebugText("Authentication successful", false, doDebug);
                string userInfo = "Username: " + Social.localUser.userName +
                    "\nUser ID: " + Social.localUser.id +
                    "\nIsUnderage: " + Social.localUser.underage;
                DebugSystem.UpdateDebugText(userInfo);
                UpdateLogInButton();
                CancelInvoke("LoginError");
            }
            else
            {
                DebugSystem.UpdateDebugText("Authentication failed", false, true);
                Invoke("LoginError", 5f);
            }            
        });
        DebugSystem.UpdateDebugText("Google Play LogIn, normal log in done", false, true);
#endif
    }

    private void LoadingScreenOff()
    {
        uiController.ToggleLoadingScreen(false);
        soundController.PlayMusic();
    }

    public void UpdateLogInButton()
    {
        DebugSystem.UpdateDebugText("Google Play LogIn, Update Login buttons", false, doDebug);
#if UNITY_ANDROID


        //bool active = PlayGamesPlatform.Instance.IsAuthenticated();
        bool active = Social.localUser.authenticated;
        if (active) //is logged in
        {
            DebugSystem.UpdateDebugText("Google Play LogIn, Update Login buttons to logout", false, doDebug);
            loginButton.onClick.RemoveAllListeners();
            loginButton.onClick.AddListener(() => LogOut());
            loginImage.sprite = googlePlayLogout;
        }
        else //is logged out
        {
            DebugSystem.UpdateDebugText("Google Play LogIn, Update Login buttons to login", false, doDebug);
            loginButton.onClick.RemoveAllListeners();
            bool silent = saveManager.MainSaveInfo.LoggedInStatus == 1;            
            loginButton.onClick.AddListener(() => LogIn());
            loginImage.sprite = googlePlayLogin;
        }
        achievementController.AchievButton.interactable = active;
        saveManager.AddToSave("", 0, 0);
#endif
    }

    public void LogOut()
    {
#if UNITY_ANDROID
        DebugSystem.UpdateDebugText("Google Play LogIn, log out", false, doDebug);
        PlayGamesPlatform.Instance.SignOut();
        UpdateLogInButton();        
#endif
    }

    public static int CheckLogin()
    {
#if UNITY_ANDROID
        if(Social.localUser.authenticated)
        {
            DebugSystem.UpdateDebugText("Google Play LogIn, is logged in", false, staticDoDebug);
            return 1;
        }
        DebugSystem.UpdateDebugText("Google Play LogIn, is not logged in", false, staticDoDebug);
#endif
        return 0;
    }
}
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Advertisements;

public class ADInitialization : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string gameID = "4470161";
    [SerializeField] bool testMode = true;

    [SerializeField] private BannerADController bannerAD;
    public BannerADController GetBannerAD
    {
        get
        {
            return bannerAD;
        }
    }
    [SerializeField] private BannerPosition bannerPosition;
    [SerializeField] private VideoADController videoAD;
    public VideoADController GetVideoAD
    {
        get
        {
            return videoAD;
        }
    }


    private void Awake()
    {
        Advertisement.Banner.SetPosition(bannerPosition);
        Advertisement.Initialize(gameID, testMode, this);        
    }

    public void OnInitializationComplete()
    {        
        string s = "Unity Ads initialization complete.";
        DebugSystem.UpdateDebugText(s);
        bannerAD.Init();
        videoAD.Init();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        string s = $"Unity Ads Initialization Failed: {error.ToString()} - {message}";
        DebugSystem.UpdateDebugText(s, true);
    }
}

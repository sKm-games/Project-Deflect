using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialADController : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    //https://docs.unity.com/ads/en/manual/ImplementingBasicAdsUnity

    [SerializeField] private string myPlacementID;
    [SerializeField] private int triggerTarget;
    int trigger;
    [SerializeField] private bool doDebug;

    private bool disableAds;

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        DebugSystem.UpdateDebugText($"InterstitialADController: Loading Ad: {myPlacementID}", false, doDebug);
        trigger++;
        if (trigger < triggerTarget)
        {
            return;
        }
        Advertisement.Load(myPlacementID, this);
    }

    // Show the loaded content in the Ad Unit:
    public void ShowAd()
    {
        if (disableAds)
        {
            return;
        }
        // Note that if the ad content wasn't previously loaded, this method will fail
        DebugSystem.UpdateDebugText($"InterstitialADController: Show Ad: {myPlacementID}", false, doDebug);
        Advertisement.Show(myPlacementID, this);
        trigger = 0;
    }

    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        ShowAd();
        // Optionally execute code if the Ad Unit successfully loads content.
    }

    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
        DebugSystem.UpdateDebugText($"InterstitialADController: Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}", false, doDebug);
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
    {
        DebugSystem.UpdateDebugText($"InterstitialADController: Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}", false, doDebug);
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string _adUnitId)
    {
    }
    public void OnUnityAdsShowClick(string _adUnitId)
    {
    }
    public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
    }

    public void ToggleADs()
    {
        disableAds = !disableAds;
    }
}

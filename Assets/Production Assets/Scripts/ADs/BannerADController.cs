using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerADController : MonoBehaviour
{
    //https://docs.unity.com/ads/ImplementingBannerAdsUnity.html
    [SerializeField] private string myPlacementId = "Banner_Android";

    [SerializeField] private GameObject placeHolderAD;
    [SerializeField] private bool usePlaceholder;

    private bool isActive;
    public bool GetIsActive
    {
        get
        {
            return isActive;
        }
    }
    public void Init()
    {
#if UNITY_ANDROID                 
        LoadBannerAd();
        StartCoroutine(ShowBannerWhenInitialized());
#endif
    }

    public void ToggleBannerAD(bool active)
    {
        isActive = active;
        if (!Advertisement.isInitialized)
        {
            return;
        }
        if (active)
        {
            //LoadBannerAd();
            ShowBannerAd();            
            placeHolderAD.SetActive(usePlaceholder);
#if UNITY_EDITOR
            placeHolderAD.SetActive(usePlaceholder);
#endif
        }
        else
        {
            Advertisement.Banner.Hide(true);            
            placeHolderAD.SetActive(false);
#if UNITY_EDITOR
            placeHolderAD.SetActive(false);
#endif
        }
    }

    IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.Banner.isLoaded)
        {
            Debug.LogWarning("Waiting for ad to load");
            yield return new WaitForSeconds(0.5f);
        }        
        ShowBannerAd();
#if UNITY_EDITOR
        placeHolderAD.SetActive(usePlaceholder);
#else
        placeHolderAD.SetActive(false);
#endif
    }

    public void LoadBannerAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };
        Advertisement.Banner.Load(myPlacementId, options);
    }

    // Show the loaded content in the Ad Unit:
    public void ShowBannerAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClick,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };
        Advertisement.Banner.Show(myPlacementId);
    }


    private void OnBannerShown()
    {
    
    }

    private void OnBannerHidden()
    {
    
    }

    private void OnBannerClick()
    {
    
    }

    // Implement code to execute when the loadCallback event triggers:
    void OnBannerLoaded()
    {        

    }

    // Implement code to execute when the load errorCallback event triggers:
    void OnBannerError(string message)
    {
        DebugSystem.UpdateDebugText($"Banner Error: {message}", true);        
        // Optionally execute additional code, such as attempting to load another ad.
    }    
}

using System.Collections;

using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

using TMPro;
using DG.Tweening;

public class VideoADController : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    //https://docs.unity.com/ads/ImplementingRewardedAdsUnity.html
    [SerializeField] private UIController uiController;
    [SerializeField] private GameController gameController;
    [SerializeField] private SaveManager saveMananger;
    [SerializeField] private Button gameOverVideoButton;

    private BannerADController bannerAD;

    [SerializeField] string myPlacementId = "Rewarded_Android";
    [SerializeField] int adID; //0 - game over, 1 - redo throw

    public void Init()
    {                        
        bannerAD = GetComponent<BannerADController>();
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == myPlacementId)
        {
            // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
            gameOverVideoButton.interactable = true;
        }
    }
    public void OnUnityAdsDidStart(string placementId)
    {        
        // Optional actions to take when the end-users triggers an ad.
    }
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Video loaded");        
    }
    public void OnUnityAdsShowStart(string adUnitId)
    {
    }
    public void OnUnityAdsShowClick(string adUnitId)
    {
    }

    public void LoadVideoAD()
    {
        DebugSystem.UpdateDebugText("Loading Video AD");
        Advertisement.Load(myPlacementId, this);
    }

    public void TriggerVideoAD(int id)
    {
        if (Advertisement.isInitialized)
        {
            //Advertisement.Load(myPlacementId, this);

            adID = id;
            bannerAD.ToggleBannerAD(false);
            Debug.Log("Showing Video AD");
            Advertisement.Show(myPlacementId, this);
            switch (adID)
            {
                default:
                case 0:
                    ToggleAdButton(0, false);
                    break;
            }
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId != myPlacementId)
        {
            return;
        }
        DebugSystem.UpdateDebugText("Video done" +showResult);
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.            
            VideoADDone();
            DebugSystem.UpdateDebugText("Video done");
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            DebugSystem.UpdateDebugText("Video skipped");
        }   
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Unity Ads Rewarded Ad Completed");
        // Grant a reward.
        if (placementId != myPlacementId)
        {
            return;
        }
        if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            // Reward the user for watching the ad to completion.            
            VideoADDone();
            DebugSystem.UpdateDebugText("Video done");
        }
        else if (showCompletionState.Equals(UnityAdsShowCompletionState.SKIPPED))
        {
            // Do not reward the user for skipping the ad.
            DebugSystem.UpdateDebugText("Video skipped");
        }

        // Load another ad:
        //Advertisement.Load(myPlacementId, this);
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        string s = $"VIDEO AD: Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}";
        DebugSystem.UpdateDebugText(s, true);
        // Use the error details to determine whether to try to load another ad.
        ToggleAdButton(0, false);

        //Try to load video again
        Invoke("LoadVideoAD", 4f);
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        string s = $"VIDEO AD: Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}";        
        DebugSystem.UpdateDebugText(s, true);
        // Use the error details to determine whether to try to load another ad.
    }

    public void ToggleAdButton(int ID, bool b)
    {
        GameObject ADButton = null;

        switch (ID)
        {
            default:
            case 0:
                ADButton = gameOverVideoButton.gameObject;
                break;
            case 1:               
                break;
        }

        if (b)
        {
            ADButton.SetActive(true);
            ADButton.transform.DOComplete();
            ADButton.transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.5f).SetLoops(-1, LoopType.Yoyo);            
        }
        else
        {
            ADButton.transform.DOComplete();
            ADButton.transform.DOKill();
            ADButton.transform.localScale = new Vector3(1f, 1f, 1f);
            ADButton.SetActive(false);
        }
    }

    private void VideoADDone()
    {
        switch (adID)
        {
            default:
            Debug.Log("Video AD: Invalid AD ID used: " + adID + ", not found");
                return;
            case 0:
                Type00VideoADDone();                
                break;
        }
    }

    private void Type00VideoADDone() //Double Dog Coins video ad at the end of a level
    {
        bannerAD.ToggleBannerAD(true);
        StartCoroutine(IEType00VideoADDone());
    }

    IEnumerator IEType00VideoADDone()
    {
        gameController.VideoContinue();
        yield return new WaitForSeconds(0.5f); //delay to allow video ad UI to close befor effect run, looks better
    }

}

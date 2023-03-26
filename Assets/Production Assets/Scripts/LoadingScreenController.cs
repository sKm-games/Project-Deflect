using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    private TextMeshProUGUI loadingText;    
    private CanvasGroup canvasGroup;
    private Image loadingIcon;
    private string extraString;

    [SerializeField] private bool doDebug;

    private void Awake()
    {        
        canvasGroup = GetComponent<CanvasGroup>();
        loadingText = GetComponentInChildren<TextMeshProUGUI>();
        loadingIcon = transform.GetChild(2).GetComponent<Image>();
        canvasGroup.alpha = 1;
        loadingIcon.transform.DOLocalRotate(new Vector3(0, 0, 180), 1f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        //StartCoroutine("IELoading");        
    }

    IEnumerator IELoading()
    {
        while (canvasGroup.alpha >= 0)
        {
            loadingText.text = extraString +"Loading...";
            yield return new WaitForSeconds(0.25f);
            loadingText.text = extraString + "Loading..";
            yield return new WaitForSeconds(0.25f);
            loadingText.text = extraString + "Loading.";
            yield return new WaitForSeconds(0.25f);
            loadingText.text = extraString + "Loading..";
            yield return new WaitForSeconds(0.25f);
            loadingText.text = extraString + "Loading...";            
        }   
    }

    public void ToggleLoadingScreen(bool active)
    {
        DebugSystem.UpdateDebugText("Toggle loading screen: " +active, false, doDebug);
        if (active)
        {            
            this.gameObject.SetActive(true);
            canvasGroup.DOFade(1, 0.25f);
            loadingIcon.transform.DOPlay();
            //loadingIcon.transform.DOLocalRotate(new Vector3(0, 0, 180), 0.5f).SetLoops(-1,LoopType.Incremental).SetEase(Ease.Linear);
            //StartCoroutine("IELoading");
        }
        else
        {
            canvasGroup.DOFade(0, 0.5f).OnComplete(() => LoadingOff());
        }
    }

    public void LoadingOff()
    {
        loadingIcon.transform.DOPause();                
        loadingIcon.transform.localEulerAngles = new Vector3(0, 0, 0);
        this.gameObject.SetActive(false);
        DebugSystem.UpdateDebugText("Loading screen done, system active: " + this.gameObject.activeInHierarchy, false, doDebug);
    }

    public void UpdateText(string s)
    {
        //extraString = s;
        loadingText.text = s;
    }
}

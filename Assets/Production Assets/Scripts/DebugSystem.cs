using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DebugSystem : MonoBehaviour
{
    [SerializeField] private Transform debugUI;
    private static Transform debugUIStatic;
    [SerializeField] private TextMeshProUGUI debugText;
    private static TextMeshProUGUI debugTextStatic;
    [SerializeField] private bool active;
    
    
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GraphicRaycaster graphicRaycaster;

    [SerializeField] private bool infinitLife;

    public static bool InfinitLifeStatic;

    [SerializeField] private bool autoOn;
    private static bool autoOnStatic;

    [SerializeField] private bool rayDebug;    

    private float timer;

    [SerializeField] private bool isEnabled;

    private void Awake()
    {
        Application.logMessageReceived += LogCaughtException;
        debugUIStatic = debugUI;
        debugTextStatic = debugText;
        autoOnStatic = autoOn;
        InfinitLifeStatic = infinitLife;


#if UNITY_ANDROID && !UNITY_EDITOR
        active = Debug.isDebugBuild;
#endif        
    }

    void LogCaughtException(string logText, string stackTrace, LogType logType)
    {
        if (logType == LogType.Exception)
        {
            UpdateDebugText($"Exception Error: {logText} \n StackTrace: {stackTrace}", true, true);
            // add your exception logging code here
        }
    }

    private void Start()
    {
        CheckDebugStatus();
    }

    private void Update()
    {
        if (!isEnabled)
        {
            return;
        }
        if (ReferencesController.GetUIController.CheckScreenStatus("startScreen"))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                timer = 0;
            }
            if (Input.GetButton("Fire1"))
            {                
                timer += Time.deltaTime;
                if (timer > 5)
                {
                    active = debugUI.gameObject.activeInHierarchy;
                    active = !active;
                    CheckDebugStatus();
                    timer = 0;
                }
            }
        }
        if (active && Input.GetButtonDown("Fire1") && rayDebug)
        {
            DebugUIRaycast();
        }
    }

    private void CheckDebugStatus()
    {
        debugUI.gameObject.SetActive(active);
        UpdateDebugText("Debug UI: " + active, false, true);
    }

    private static void ActivateUI()
    {
        debugUIStatic.gameObject.SetActive(true);
        UpdateDebugText("Debug UI: " + true, false, true);
    }

    public void HideDebugUI()
    {
        active = false;
        debugUI.gameObject.SetActive(active);
        UpdateDebugText("Debug UI: " + active, false, true);
    }

    public static void UpdateDebugText(string s, bool f = false, bool d = true)
    {
        if (!d && !f)
        {
            return;
        }

        if (debugTextStatic == null)
        {
            return;
        }
        debugTextStatic.text = s + "\n---------\n" + debugTextStatic.text;

        if (f && autoOnStatic)
        {
            ActivateUI();
        }
        Debug.Log(s);
    }

    public void DebugClearText()
    {
        Debug.LogWarning("Clear debug");
        debugTextStatic.text = "Debug:\n";
    }

    public void DebugToggleInfinitLevel(TextMeshProUGUI t)
    {
        infinitLife = !infinitLife;
        InfinitLifeStatic = infinitLife;
        t.text = $"Gode Mode: \n {infinitLife}";
    }

    public void GooglePlayLogOut()
    {
        ReferencesController.GetGooglePlayController.LogOut();
    }

    private void DebugUIRaycast()
    {
        PointerEventData pEventData = new PointerEventData(eventSystem);
        pEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        graphicRaycaster.Raycast(pEventData, results);


        foreach (RaycastResult rr in results)
        {
            string s = "Ray hit: " + rr.gameObject.name + " in " + rr.gameObject.transform.parent.name;
            UpdateDebugText(s);
        }
        
    }

    private void OnValidate()
    {
        InfinitLifeStatic = infinitLife;
    }
}

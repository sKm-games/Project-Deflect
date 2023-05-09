using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScreenshots : MonoBehaviour
{   
    [SerializeField] private bool active = false;
    private static bool activeStatic;
    private static int index;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && active)
        {
            TakeDebugScreenShoot();
        }
    }

    public static void TakeDebugScreenShoot(string n = "Screenshot")
    {
#if UNITY_EDITOR   
        if (!activeStatic)
        {
            return;
        }

        string name = $"{n}-{index}";

        ScreenCapture.CaptureScreenshot($"{name}.png");
        Debug.Log($"Screenshot taken: {name}.png");
        
        index++;
#endif
    }

    private void OnValidate()
    {
        activeStatic = active;
    }
}

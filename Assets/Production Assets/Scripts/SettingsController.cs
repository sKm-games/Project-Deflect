using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] private Slider sfxSliderPause;
    private TextMeshProUGUI sfxSliderPauseText;    
    [SerializeField] private Slider sfxSliderSettings;
    private TextMeshProUGUI sfxSliderSettingsText;
    
    [Header("Music")]
    [SerializeField] private Slider musicSliderPause;
    private TextMeshProUGUI musicSliderTextPause;
    [SerializeField] private Slider musicSliderSettings;
    private TextMeshProUGUI musicSliderTextSettings;

    [Header("Controls")]
    [SerializeField] private Slider sensitivitySliderPause;
    private TextMeshProUGUI sensitivitySliderTextPause;
    [SerializeField] private Slider sensitivitySliderSettings;
    private TextMeshProUGUI sensitivitySliderTextSettings;

    [Header("Other")]
    [SerializeField] private Toggle countdownAlarmToggleSettings;
    [SerializeField] private Toggle countdownAlarmTogglePause;
    private bool allowAlarm;
    public bool GetAllowAlarm
    {
        get
        {
            return allowAlarm;
        }
    }

    [SerializeField] private TMP_Dropdown vibrationDropdownSettings;
    [SerializeField] private TMP_Dropdown vibrationDropdownPause;
    private VibrationEnums vibrationEnum;

    private void Awake()
    {
        sfxSliderPauseText = sfxSliderPause.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        sfxSliderSettingsText = sfxSliderSettings.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        musicSliderTextPause = musicSliderPause.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        musicSliderTextSettings = musicSliderSettings.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        sensitivitySliderTextPause = sensitivitySliderPause.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        sensitivitySliderTextSettings = sensitivitySliderSettings.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void UpdateSoundEffects(Slider s)
    {
        s.value = (float)decimal.Round((decimal)s.value, 2);

        sfxSliderPause.SetValueWithoutNotify(s.value);
        sfxSliderPauseText.text = $"SFX: {s.value * 100}%";

        sfxSliderSettings.SetValueWithoutNotify(s.value);
        sfxSliderSettingsText.text = $"SFX: {s.value * 100}%";

        ReferencesController.GetSoundController.UpdateSFX(s.value);
    }

    public void UdapteMusic(Slider s)
    {
        s.value = (float)decimal.Round((decimal)s.value, 2);

        musicSliderPause.SetValueWithoutNotify(s.value);
        musicSliderTextPause.text = $"Music: {s.value * 100}%";
        musicSliderSettings.SetValueWithoutNotify(s.value);
        musicSliderTextSettings.text = $"Music: {s.value * 100}%";

        ReferencesController.GetSoundController.UpdateMusic(s.value);
    }

    public void UpdateSensetivity(Slider s)
    {
        s.value = (float)decimal.Round((decimal)s.value, 1);
        sensitivitySliderPause.SetValueWithoutNotify(s.value);
        sensitivitySliderTextPause.text = $"Sensetivity: {s.value}";
        sensitivitySliderSettings.SetValueWithoutNotify(s.value);
        sensitivitySliderTextSettings.text = $"Sensetivity: {s.value}";

        ReferencesController.GetBlockerController.UpdateSensitivity(s.value);
    }

    public void UpdateAlarm(Toggle t)
    {
        allowAlarm = t.isOn;
        countdownAlarmTogglePause.SetIsOnWithoutNotify(allowAlarm);
        countdownAlarmToggleSettings.SetIsOnWithoutNotify(allowAlarm);
    }

    public void UpdateVibration(TMP_Dropdown d)
    {        
        vibrationEnum = (VibrationEnums)d.value;
#if UNITY_ANDROID
        if (SystemInfo.deviceType != DeviceType.Handheld)
        {
            return;
        }
        switch (vibrationEnum)
        {
            case VibrationEnums.On:         
            case VibrationEnums.Hit:                
            case VibrationEnums.Shot:
                Handheld.Vibrate();
                break;
            case VibrationEnums.Off:
                break;
            default:
                break;
        }
#endif
    }


    public void CheckVibration(VibrationTriggerEnums t)
    {
#if UNITY_ANDROID
        if (SystemInfo.deviceType != DeviceType.Handheld)
        {
            return;
        }
        switch (vibrationEnum)
        {
            case VibrationEnums.On:                
                Handheld.Vibrate();
                break;
            case VibrationEnums.Hit:
                if (t == VibrationTriggerEnums.Hit)
                {
                    Handheld.Vibrate();
                }
                break;
            case VibrationEnums.Shot:
                if (t == VibrationTriggerEnums.Shot)
                {
                    Handheld.Vibrate();
                }
                break;
            case VibrationEnums.Off:
                break;
            default:
                break;
        }
#endif
    }

    public void SetSaveInfo(float s, float m, float sens, int a, int d)
    {
        sfxSliderPause.SetValueWithoutNotify(s);
        sfxSliderPauseText.text = $"SFX: {s * 100}%";
        sfxSliderSettings.SetValueWithoutNotify(s);
        sfxSliderSettingsText.text = $"SFX: {s * 100}%";
        ReferencesController.GetSoundController.UpdateSFX(s);

        musicSliderPause.SetValueWithoutNotify(m);
        musicSliderTextPause.text = $"Music: {m * 100}%";
        musicSliderSettings.SetValueWithoutNotify(m);
        musicSliderTextSettings.text = $"Muisc: {m * 100}%";

        ReferencesController.GetSoundController.UpdateMusic(m);

        sensitivitySliderPause.SetValueWithoutNotify(sens);
        sensitivitySliderSettings.SetValueWithoutNotify(sens);
        ReferencesController.GetBlockerController.UpdateSensitivity(sens);

        countdownAlarmTogglePause.SetIsOnWithoutNotify(a == 1);
        countdownAlarmToggleSettings.SetIsOnWithoutNotify(a == 1);
        allowAlarm = a == 1;

        vibrationDropdownPause.SetValueWithoutNotify(d);
        vibrationDropdownSettings.SetValueWithoutNotify(d);
        vibrationEnum = (VibrationEnums)vibrationDropdownPause.value;        
    }

    public void GetSaveInfo(out float s, out float m, out float sens, out int a, out int d)
    {
        s = sfxSliderPause.value;
        m = musicSliderPause.value;
        sens = sensitivitySliderPause.value;
        a = countdownAlarmTogglePause.isOn ? 1 : 0;
        d = vibrationDropdownPause.value;
    }

}

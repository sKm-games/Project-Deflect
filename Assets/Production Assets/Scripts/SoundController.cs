using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SoundController : MonoBehaviour
{
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private List<SoundDataClass> soundEffects;
    [SerializeField] private Toggle sfxTogglePause;
    [SerializeField] private Toggle sfxToggleSettings;
    private bool sfxStatus;
    public int GetSFXStatus
    {
        get
        {
            return sfxStatus ? 1 : 0;
        }
    }
    [SerializeField] private List<AudioSource> sfxAudiosources;

    [SerializeField] private Slider sfxSliderPause;
    private TextMeshProUGUI sfxSliderPauseText;
    [SerializeField] private Slider sfxSliderSettings;
    private TextMeshProUGUI sfxSliderSettingsText;
    [SerializeField] private Slider musicSliderPause;
    private TextMeshProUGUI musicSliderTextPause;
    [SerializeField] private Slider musicSliderSettings;
    private TextMeshProUGUI musicSliderTextSettings;

    private bool musicStatus;
    public int GetMusicStatus
    {
        get
        {
            return musicStatus ? 1 : 0;
        }
    }
    private AudioSource musicAudiosource;

    [SerializeField] private float fadeSpeed;
    [SerializeField] private bool doDebug;

    private void Awake()
    {
        musicAudiosource = this.GetComponents<AudioSource>()[0];
        sfxAudiosources = new List<AudioSource>(this.GetComponentsInChildren<AudioSource>());
        sfxAudiosources.RemoveAt(0); //remove music
        sfxSliderPauseText = sfxSliderPause.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        sfxSliderSettingsText = sfxSliderSettings.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        musicSliderTextPause = musicSliderPause.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        musicSliderTextSettings = musicSliderSettings.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void PlaySFX(string id)
    {
        id = id.ToLower();
        SoundDataClass s = soundEffects.Find((x) => x.ID == id);

        if (s == null)
        {            
            return;
        }

        if (CheckIfLooping(s))
        {
            return;
        }

        float p = Random.Range(s.pitchRange.x, s.pitchRange.y);
        AudioSource a = GetSFXSource();

        a.volume = sfxSliderPause.value > s.maxVolume ? s.maxVolume : sfxSliderPause.value;
        a.loop = s.loop;
        a.pitch = p;
        a.clip = s.Clip;
        a.Play();
    }

    private bool CheckIfLooping(SoundDataClass s)
    {        

        AudioSource a = sfxAudiosources.Find((x) => x.clip == s.Clip && x.loop);

        return a != null;
    }

    public void StopLoopSFX(string id)
    {
        SoundDataClass s = soundEffects.Find((y) => y.ID == id);
        AudioSource a = sfxAudiosources.Find((x) => x.clip == s.Clip && x.loop);

        if (a == null)
        {
            Debug.LogWarning($"SoundController: StopLoppSFX: source not found for sound {id}");
            return;
        }
        a.Stop();
    }

    public void PlayMusic()
    {        
        musicAudiosource.Play();
        //float v = musicStatus ? musicMax : 0;

        //musicAudiosource.DOFade(v, 0.5f);
        musicAudiosource.DOFade(musicSliderPause.value, 0.5f);
    }    

    /*public void ToggleSFX(Toggle t)
    {
        sfxStatus = t.isOn;
        sfxTogglePause.SetIsOnWithoutNotify(sfxStatus);
        sfxToggleSettings.SetIsOnWithoutNotify(sfxStatus);

        foreach (AudioSource a in sfxAudiosources)
        {
            if (!sfxStatus)
            {
                a.DOFade(0, fadeSpeed);
                continue;
            }

            SoundDataClass s = soundEffects.Find((x) => x.Clip == a.clip);

            if (s == null)
            {
                continue;
            }
            
            a.DOFade(s.maxVolume, fadeSpeed);
        }        
    }

    public void ToggleMusic(Toggle t)
    {
        musicStatus = t.isOn;
        musicTogglePause.SetIsOnWithoutNotify(musicStatus);
        musicToggleSettings.SetIsOnWithoutNotify(musicStatus);
        float v = musicStatus ? musicMax : 0f;

        if (musicStatus && !musicAudiosource.isPlaying)
        {
            PlayMusic();
        }
        DOTween.timeScale = 1;
        musicAudiosource.DOFade(v, fadeSpeed);        
    }*/

    public void SFXSlider(Slider s)
    {
        s.value = (float)decimal.Round((decimal)s.value, 2);

        foreach (AudioSource a in sfxAudiosources)
        {
            a.volume = s.value;
        }

        sfxSliderPause.SetValueWithoutNotify(s.value);
        sfxSliderPauseText.text = $"SFX: {s.value*100}%";       

        sfxSliderSettings.SetValueWithoutNotify(s.value);
        sfxSliderSettingsText.text = $"SFX: {s.value*100}%";

        if (!sfxAudiosources[0].isPlaying)
        {
            PlaySFX(soundEffects[0].ID);
        }        
    }

    public void MusicSlider(Slider s)
    {
        s.value = (float)decimal.Round((decimal)s.value, 2);

        musicAudiosource.volume = s.value;

        musicSliderPause.SetValueWithoutNotify(s.value);
        musicSliderTextPause.text = $"Music: {s.value * 100}%";
        musicSliderSettings.SetValueWithoutNotify(s.value);
        musicSliderTextSettings.text = $"Muisc: {s.value * 100}%";
    }

    //public void SetSaveInfo(int s, int m)
    public void SetSaveInfo(float s, float m)
    {
        DebugSystem.UpdateDebugText("Set Sound Save Info", false, doDebug);
        #region Toggles
        /* sfxStatus = s == 1 ? true : false;

         float v = 0;
         foreach (AudioSource a in sfxAudiosources)
         {
             if (!sfxStatus)
             {
                 a.DOFade(0, fadeSpeed);
                 continue;
             }

             SoundDataClass sc = soundEffects.Find((x) => x.Clip == a.clip);

             if (sc == null)
             {
                 continue;
             }

             a.DOFade(sc.maxVolume, fadeSpeed);
         }

         sfxTogglePause.SetIsOnWithoutNotify(sfxStatus);       
         sfxToggleSettings.SetIsOnWithoutNotify(sfxStatus);

         musicStatus = m == 1 ? true : false;
         v = musicStatus ? musicMax : 0f;
         musicAudiosource.DOFade(v, fadeSpeed);

         musicTogglePause.SetIsOnWithoutNotify(musicStatus);
         musicToggleSettings.SetIsOnWithoutNotify(musicStatus);*/
        #endregion
        sfxSliderPause.SetValueWithoutNotify(s);
        sfxSliderPauseText.text = $"SFX: {s * 100}%";

        sfxSliderSettings.SetValueWithoutNotify(s);
        sfxSliderSettingsText.text = $"SFX: {s * 100}%";

        musicSliderPause.SetValueWithoutNotify(m);
        musicSliderTextPause.text = $"Music: {m * 100}%";
        musicSliderSettings.SetValueWithoutNotify(m);
        musicSliderTextSettings.text = $"Muisc: {m * 100}%";
    }

    public void GetSaveInfo(out float s, out float m)
    {
        s = sfxSliderPause.value;
        m = musicSliderPause.value;
    }

    private AudioSource GetSFXSource()
    {
        foreach (AudioSource a in sfxAudiosources)
        {
            if (!a.isPlaying)
            {
                return a;
            }
        }

        return sfxAudiosources[0];
    }
}

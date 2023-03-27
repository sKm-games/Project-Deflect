using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    
    [SerializeField] private Toggle musicTogglePause;
    [SerializeField] private Toggle musicToggleSettings;
    [SerializeField] private float musicMax;
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

    private void Awake()
    {
        musicAudiosource = this.GetComponents<AudioSource>()[0];
        sfxAudiosources = new List<AudioSource>(this.GetComponentsInChildren<AudioSource>());
        sfxAudiosources.RemoveAt(0); //remove music
    }

    public void PlaySFX(string id)
    {
        SoundDataClass s = soundEffects.Find((x) => x.ID == id);

        if (s == null)
        {
            Debug.LogWarning($"SoundController: PlaySFX: soundData not found for sound {id}");
        }

        if (CheckIfLooping(s))
        {
            return;
        }

        float p = Random.Range(s.pitchRange.x, s.pitchRange.y);
        AudioSource a = GetSFXSource();

        a.volume = sfxStatus ? s.maxVolume : 0;
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
        }
        a.Stop();
    }

    public void PlayMusic()
    {
        musicAudiosource.Play();
        float v = musicStatus ? musicMax : 0;
        musicAudiosource.DOFade(v, 0.5f);
    }    

    public void ToggleSFX(Toggle t)
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
    }

    public void SetSaveInfo(int s, int m)
    {
        Debug.Log("SetSoundSaveInfo");
        sfxStatus = s == 1 ? true : false;

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
        v = musicStatus ? 0f : musicMax;
        musicAudiosource.DOFade(v, fadeSpeed);

        musicTogglePause.SetIsOnWithoutNotify(musicStatus);
        musicToggleSettings.SetIsOnWithoutNotify(musicStatus);

        if (musicStatus)
        {
            PlayMusic();
        }
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

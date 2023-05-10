using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SoundController : MonoBehaviour
{
    [SerializeField] private List<SoundDataClass> soundEffects;
    [SerializeField] private List<AudioSource> sfxAudiosources;

    private float sfxVolume;
    private float musicVolume;

    private AudioSource musicAudiosource;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private bool doDebug;

    private void Awake()
    {
        musicAudiosource = this.GetComponents<AudioSource>()[0];
        sfxAudiosources = new List<AudioSource>(this.GetComponentsInChildren<AudioSource>());
        sfxAudiosources.RemoveAt(0); //remove music

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

        a.volume = sfxVolume > s.maxVolume ? s.maxVolume : sfxVolume;
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

    public void StopLoopSFX(string id, bool force)
    {
        SoundDataClass s = soundEffects.Find((y) => y.ID == id);
        AudioSource a = sfxAudiosources.Find((x) => x.clip == s.Clip && x.loop);

        if (a == null)
        {            
            return;
        }

        if (force)
        {
            //a.DOFade(0, 0.125f).OnComplete(() => a.Stop());
            a.Stop();
        }

        a.loop = false;
    }

    public void PlayMusic()
    {        
        musicAudiosource.Play();
        //float v = musicStatus ? musicMax : 0;

        //musicAudiosource.DOFade(v, 0.5f);
        musicAudiosource.DOFade(musicVolume, 0.5f);
    }    
        
    public void UpdateSFX(float v)
    {
        v = (float)decimal.Round((decimal)v, 2);

        foreach (AudioSource a in sfxAudiosources)
        {
            a.volume = v;
        }

        if (!sfxAudiosources[0].isPlaying)
        {
            PlaySFX(soundEffects[0].ID);
        }

        sfxVolume = v;
    }

    public void UpdateMusic(float v)
    {
        v = (float)decimal.Round((decimal)v, 2);

        musicAudiosource.volume = v;

        musicVolume = v;
    }

    //public void SetSaveInfo(int s, int m)

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

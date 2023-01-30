using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer audioMixer;

    public Sound[] musicSounds, sfxSounds, dubbingSounds;
    public AudioSource musicSource, sfxSource, dubbingSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("MainTheme");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s==null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }
    public void PlaySFX(string name)
    {

        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
    public void PlayDubbing(string name) 
    {
        Sound s = Array.Find(dubbingSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            dubbingSource.PlayOneShot(s.clip);
        }
    }

    public void ToggleMusic(Image image)
    {
        musicSource.mute= !musicSource.mute;
        if (musicSource.mute)
        {
            image.color = Color.grey;
        }
        else
        {
            image.color = Color.white;
        }
    }

    public void ToggleSFX(Image image)
    {
        sfxSource.mute= !sfxSource.mute;
        if (sfxSource.mute)
        {
            image.color = Color.grey;
        }
        else
        {
            image.color = Color.white;
        }
    }

    public void ToggleDubbing(Image image)
    {
        dubbingSource.mute= !dubbingSource.mute;
        if (dubbingSource.mute)
        {
            image.color = Color.grey;
        }
        else
        {
            image.color = Color.white;
        }
    }

    public void MusicVolume(float volume)
    {
        audioMixer.SetFloat("VolumeMusic", ParseToDebit20(volume));
        //musicSource.volume = volume;
    }
    
    public void SFXVolume(float volume)
    {
        audioMixer.SetFloat("VolumeSFX", ParseToDebit20(volume));
        //sfxSource.volume = volume;
    }

    public void DubbingVolume(float volume)
    {
        audioMixer.SetFloat("VolumeDubbing", ParseToDebit20(volume));
        //dubbingSource.volume = volume;
    }

    public static float ParseToDebit0(float value)
    {
        float parse = Mathf.Lerp(-80, 00, Mathf.Clamp01(value));
        return parse;
    }

    public static float ParseToDebit20(float value)
    {
        float parse = Mathf.Lerp(-80, 20, Mathf.Clamp01(value));
        return parse;
    }

    public static float ParseToDebitCustom(float value, float min = -80, float max = 20)
    {
        float parse = Mathf.Lerp(min, max, Mathf.Clamp01(value));
        return parse;
    }
}

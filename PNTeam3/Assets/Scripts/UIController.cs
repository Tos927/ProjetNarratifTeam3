using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider musicSlider, sfxSlider, dubbingSlider;

    private void Start()
    {
        GameManager.instance.ShowUI();
    }

    public void ToggleMusic()
    {
        AudioManager.instance.ToggleMusic();
    }

    public void ToggleSFX()
    {
        AudioManager.instance.ToggleSFX();
    }
    public void ToggleDubbing()
    {
        AudioManager.instance.ToggleDubbing();
    }

    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(sfxSlider.value);
    }

    public void DubbingVolume()
    {
        AudioManager.instance.DubbingVolume(dubbingSlider.value);
    }
}

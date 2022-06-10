using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public bool isMusic;

    public void SetVolume(float volume)
    {
        if (isMusic)
        {
            Audio_Manager.i.SetMusicVolume(volume);
        }
        else
        {
            Audio_Manager.i.SetSFXVolume(volume);
        }
    }

    private void OnEnable()
    {
        if (isMusic)
        {
            GetComponent<Slider>().value = Audio_Manager.i.volumeMusic;
        }
        else
        {
            GetComponent<Slider>().value = Audio_Manager.i.volumeSFX;
        }
    }

    private void OnDisable()
    {
        if (isMusic)
        {
            Save_Manager.i.Save();
        }
    }
}
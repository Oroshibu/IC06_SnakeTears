using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Audio_Manager : MonoBehaviour
{
    private static Audio_Manager _i;

    public float volumeMusic = .75f;
    public float volumeSFX = .75f;

    private AudioSource musicSource;    
    private AudioSource sfxSource;
    
    [System.Serializable]
    public struct SFX
    {
        public string name;
        public AudioClip clip;
        public float volume;
        public float pitchVariation;
    }

    public List<AudioClip> musics;
    private int playingMusicIndex = -1;
    public List<SFX> SFXs;
    
    public static Audio_Manager i
    {
        get
        {
            if (_i != null)
            {
                return _i;
            }
            else
            {
                return _i = new GameObject("Audio_Manager").AddComponent<Audio_Manager>();
            }
        }
    }

    private void Awake()
    {
        if (_i == null && _i != this)
        {
            _i = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Init()
    {
        //get audio sources
        AudioSource[] sources = GetComponents<AudioSource>();
        musicSource = sources[0];
        sfxSource = sources[1];
        Save_Manager.i.Load();
    }

    public void PlayMusic(int index)
    {
        if (index < musics.Count && playingMusicIndex != index)
        {
            if (playingMusicIndex == -1)
            {
                playingMusicIndex = index;
                musicSource.volume = 0;
                musicSource.DOFade(volumeMusic * .5f, 2);
                musicSource.clip = musics[index];
                musicSource.Play();
            }
            else
            {
                //    musicSource.volume = 0;
                //    musicSource.DOFade(volumeMusic * .5f, 2);
                musicSource.clip = musics[index];
                musicSource.Play();
                playingMusicIndex = index;
                //musicSource.DOFade(0, 2).OnComplete(() => {
                //    musicSource.volume = 0;
                //    musicSource.DOFade(volumeMusic * .5f, 2);
                //    musicSource.clip = musics[index];
                //    musicSource.Play();
                //}
                //);

            }
        }
    }

    public void MusicFadeOut(float duration = 2)
    {
        musicSource.DOFade(0, duration);
    }

    public void MusicFadeIn(float duration = 2)
    {
        musicSource.DOFade(volumeMusic * .5f, duration);
    }

    public void PlaySound(string name)
    {
        foreach (SFX sfx in SFXs)
        {
            if (sfx.name == name)
            {
                sfxSource.pitch = 1f + Random.Range(-sfx.pitchVariation, sfx.pitchVariation);
                sfxSource.PlayOneShot(sfx.clip, sfx.volume * volumeSFX);
                break;
            }
        }
    }

    public void SetMusicVolume(float volume)
    {
        volumeMusic = volume;
        musicSource.volume = volumeMusic * .5f;
    }

    public void SetSFXVolume(float volume)
    {
        volumeSFX = volume;
    }
}

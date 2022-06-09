using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    private static Audio_Manager _i;

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

    public List<AudioClip> Musics;
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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //get audio sources
        AudioSource[] sources = GetComponents<AudioSource>();
        musicSource = sources[0];
        sfxSource = sources[1];
    }

    public void PlayMusic(int index)
    {
        if (index < Musics.Count && playingMusicIndex != index)
        {
            playingMusicIndex = index;
            musicSource.clip = Musics[index];
            musicSource.Play();
        }
    }

    public void PlaySound(string name)
    {
        foreach (SFX sfx in SFXs)
        {
            if (sfx.name == name)
            {
                sfxSource.pitch = 1f + Random.Range(-sfx.pitchVariation, sfx.pitchVariation);
                sfxSource.PlayOneShot(sfx.clip, sfx.volume);
                break;
            }
        }
    }
}

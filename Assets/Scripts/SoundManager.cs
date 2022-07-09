using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] public AudioSource musicSource, effectsSource, playerSource;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        effectsSource.PlayOneShot(clip);
    }
    
    public void PlayPlayerSound(AudioClip clip)
    {
        playerSource.PlayOneShot(clip);
    }

    public bool ToggleSFX()
    {
        if (effectsSource.mute && playerSource.mute)
        {
            effectsSource.mute = false;
            playerSource.mute = false;
            return true;
        }
        else
        {
            effectsSource.mute = true;
            playerSource.mute = true;
            return false;
        }
    }

    public bool ToggleMusic()
    {
        if (musicSource.mute)
        {
            musicSource.mute = false;
            return true;
        }
        else
        {
            musicSource.mute = true;
            return false;
        }
    }

    public void ChangeMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}

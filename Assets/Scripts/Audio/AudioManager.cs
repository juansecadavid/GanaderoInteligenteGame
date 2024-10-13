using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;

    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioClip[] soundClips;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayMusic(int index, bool loop)
    {
        musicSource.clip = musicClips[index];
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlaySound(int index, bool loop)
    {
        soundSource.clip = soundClips[index];
        soundSource.loop = loop;
        soundSource.Play();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void PauseSound()
    {
        soundSource.Pause();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void StopSound()
    {
        soundSource.Stop();
    }
}

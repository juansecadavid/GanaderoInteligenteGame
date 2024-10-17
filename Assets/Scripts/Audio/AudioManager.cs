using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioClip[] soundClips;

    public static AudioManager Instance { get; private set; }

    private Queue<AudioSource> audioSourcePool;
    private List<AudioSource> activeAudioSources;
    [SerializeField] private int poolSize = 6;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            InitializePool();
        }
    }

    private void InitializePool()
    {
        audioSourcePool = new Queue<AudioSource>();
        activeAudioSources = new List<AudioSource>();

        for (int i = 0; i < poolSize; i++)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            audioSourcePool.Enqueue(newSource);
        }
    }

    public void Initialize()
    {
        Cow.cowHit += ShouldPlaySound;
    }

    void ShouldPlaySound()
    {
        print("Llame a que suene");
        if (Random.Range(0, 3) == 0)
        {
            PlayRandomSound();
        }
    }

    void PlayRandomSound()
    {
        int randomIndex = Random.Range(0, soundClips.Length);
        PlaySound(randomIndex, false);
    }

    public void PlayMusic(int index, bool loop)
    {
        musicSource.clip = musicClips[index];
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlaySound(int index, bool loop)
    {
        if (audioSourcePool.Count > 0)
        {
            AudioSource source = audioSourcePool.Dequeue();
            activeAudioSources.Add(source);

            source.clip = soundClips[index];
            source.loop = loop;
            source.Play();

            StartCoroutine(ReturnToPoolWhenFinished(source));
        }
        else
        {
            Debug.LogWarning("No available AudioSources in the pool.");
        }
    }

    private IEnumerator ReturnToPoolWhenFinished(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);

        source.clip = null;
        activeAudioSources.Remove(source);
        audioSourcePool.Enqueue(source);
    }

    public void PauseAllAudioSources()
    {
        foreach (var source in activeAudioSources)
        {
            if (source.isPlaying)
            {
                source.Pause();
            }
        }
        musicSource.Pause();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void Conclude()
    {
        Cow.cowHit -= ShouldPlaySound;
    }
}

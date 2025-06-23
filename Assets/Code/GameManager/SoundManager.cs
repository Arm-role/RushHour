using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioClip[] audioClips;

    public Dictionary<string, AudioClip> soundClips = new Dictionary<string, AudioClip>();

    private AudioSource audioSource;
    public GameObject SoundSpawn;

    public float SFXVolum = 1f;
    public float musicVolum = 0.3f;
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
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;

        foreach (AudioClip clip in audioClips)
        {
            if (!soundClips.ContainsKey(clip.name))
            {
                soundClips.Add(clip.name, clip);
            }
        }
    }
    private void Start()
    {
        PlayLoopClip("startScene");
    }
    public void PlayLoopClip(string clipIndex)
    {
        if (soundClips.ContainsKey(clipIndex))
        {
            audioSource.volume = musicVolum;
            audioSource.clip = soundClips[clipIndex];
            audioSource.Play();
        }
    }
    public void StopLoopClip()
    {
        audioSource.Stop();
    }
    public void CreateDestroyableSound(string clipIndex)
    {
        if (!soundClips.ContainsKey(clipIndex) || SoundSpawn == null)
        {
            Debug.LogWarning($"Sound clip with index '{clipIndex}' not found or SoundSpawn is null.");
            return;
        }

        GameObject soundObject = Instantiate(SoundSpawn);
        AudioSource source = soundObject.GetComponent<AudioSource>();

        source.volume = SFXVolum;
        source.clip = soundClips[clipIndex];
        source.Play();

        Destroy(soundObject, source.clip.length);
    }

    public GameObject CreatePersistentSound(string clipIndex)
    {
        if (!soundClips.ContainsKey(clipIndex) || SoundSpawn == null)
        {
            Debug.LogWarning($"Sound clip with index '{clipIndex}' not found or SoundSpawn is null.");
            return null;
        }

        GameObject soundObject = Instantiate(SoundSpawn);
        AudioSource source = soundObject.GetComponent<AudioSource>();

        source.volume = SFXVolum;
        source.clip = soundClips[clipIndex];
        source.Play();

        return soundObject;
    }
    public void SetSFXVolum(Scrollbar bar)
    {
        SFXVolum = bar.value;
        CreateDestroyableSound("cutting");
    }
    public void SetMusicVolum(Scrollbar bar)
    {
        musicVolum = bar.value;
    }
}

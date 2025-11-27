using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundLibrary", menuName = "Library/SoundLibrary")]
public class SoundLibrary : ScriptableObject
{
    public List<SoundEntry> Entries;

    public SoundEntry Get(string id)
    {
        return Entries.Find(e => e.Id == id);
    }
}


[Serializable]
public class SoundEntry
{
    public string Id;
    public AudioClip Clip;
    [Range(0f, 1f)] public float Volume = 1f;
    public bool Loop;
}

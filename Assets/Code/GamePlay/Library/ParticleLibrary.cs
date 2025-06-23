using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "new ParticleLibrary", menuName = "Library/ParticleLibrary")]
public class ParticleLibrary : ScriptableObject
{
    public List<ParticleEntry> Entries;

    public AssetReferenceGameObject Find(string path)
    {
        return Entries.Find(e => e.Name == path).Adressable;
    }
}
[Serializable]
public struct ParticleEntry
{
    public string Name;
    public AssetReferenceGameObject Adressable;
}
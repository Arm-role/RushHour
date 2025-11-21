using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MenuLibrary", menuName = "Library/MenuLibrary")]
public class MenuLibrary : ScriptableObject
{
    public List<ObjectEntry<Menu>> Entries;

    private void OnValidate()
    {
        foreach (var entry in Entries)
        {
            entry.Id = entry.Adressable.ID;
            entry.Name = entry.Adressable.Name;
        }
    }
    public Menu Find(string friendlyName)
    {
        var entries = Entries.Find(l => friendlyName == l.Name);
        return entries.Adressable;
    }
    public Menu Find(int id)
    {
        var entries = Entries.Find(l => id == l.Id);
        return entries.Adressable;
    }
    public int FindIdByName(string friendlyName)
    {
        return Entries.Find(e => e.Name == friendlyName).Id;
    }
    public string FindNameById(int id)
    {
        return Entries.Find(e => e.Id == id).Name;
    }
}
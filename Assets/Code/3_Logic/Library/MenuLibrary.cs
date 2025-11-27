using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MenuLibrary", menuName = "Library/MenuLibrary")]
public class MenuLibrary : ScriptableObject
{
    public List<Menu> Entries;

    public Menu Find(string friendlyName)
    {
        return Entries.Find(menu => menu.Name == friendlyName);
    }
    public Menu Find(int id)
    {
        return Entries.Find(menu => menu.ID == id);
    }
    public int FindIdByName(string friendlyName)
    {
        return Entries.Find(menu => menu.Name == friendlyName).ID;
    }
    public string FindNameById(int id)
    {
        return Entries.Find(menu => menu.ID == id).Name;
    }
}

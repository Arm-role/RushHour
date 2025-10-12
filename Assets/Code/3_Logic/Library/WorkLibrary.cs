using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkLibrary", menuName = "Library/WorkLibrary")]
public class WorkLibrary : ScriptableObject
{
    [Serializable]
    public class WorkEntry
    {
        public string InputItemName;  // เช่น Bread = 1001
        public string ToolItemName;   // เช่น Pan = 1000
        public string OutputItemName; // เช่น BreadCook = 1002
    }

    [SerializeField] private List<WorkEntry> _entries = new List<WorkEntry>();
    private Dictionary<(string InputItemName, string ToolItemName), string> _lookupCookItem;
    private Dictionary<string, (string InputItemName, string ToolItemName)> _lookupItem;


    public void Initialize()
    {
        _lookupCookItem = new Dictionary<(string, string), string>();
        foreach (var entry in _entries)
        {
            var key = (entry.InputItemName, entry.ToolItemName);
            if (!_lookupCookItem.ContainsKey(key))
            {
                _lookupCookItem.Add(key, entry.OutputItemName);
            }
        }

        _lookupItem = new Dictionary<string, (string, string)>();
        foreach (var entry in _entries)
        {
            if (!_lookupItem.ContainsKey(entry.OutputItemName))
            {
                var value = (entry.InputItemName, entry.ToolItemName);
                _lookupItem.Add(entry.OutputItemName, value);
            }
        }
    }

    public string GetToolName(string cookName)
    {
        if (_lookupItem.TryGetValue(cookName, out var value) && value != (null, null))
        {
            return value.ToolItemName;
        }

        return string.Empty;
    }
}
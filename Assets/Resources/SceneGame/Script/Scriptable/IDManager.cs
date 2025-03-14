using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDManager
{
    private static IDManager instance;
    public static IDManager Instance => instance ??= new IDManager();

    public IDManager() { }

    private Dictionary<Type, HashSet<int>> IDList = new Dictionary<Type, HashSet<int>>();
    private Dictionary<Type, int> IDIndex = new Dictionary<Type, int>();

    public int GenerateNewID<T>()
    {
        Type type = typeof(T);

        if (!IDList.ContainsKey(type))
        {
            IDList[type] = new HashSet<int>();
            IDIndex[type] = 1001;
        }
        while (IDList[type].Contains(IDIndex[type]))
        {
            IDIndex[type]++;

            if (IDIndex[type] > 9999)
            {
                IDIndex[type] = 1001;
            }
        }

        IDList[type].Add(IDIndex[type]);
        return IDIndex[type];
    }
    public void ResetIDs<T>()
    {
        Type type = typeof(T);

        IDList[type].Clear();
        IDIndex[type] = 1001;
    }
    public void RemoveID<T>(int id)
    {
        Type type = typeof(T);

        if (IDList.ContainsKey(type))
        {
            if (IDList[type].Contains(id))
            {
                IDList[type].Remove(id);
                Debug.Log($"✅ Removed ID {id} from {type.Name}");
            }
        }
    }
}

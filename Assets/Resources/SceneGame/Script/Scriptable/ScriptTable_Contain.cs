using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public interface IIdentifiable
{
    int ID { get; }
    string Name { get; }
}

public class ScriptTable_Contain : MonoBehaviour
{
    public static ScriptTable_Contain instance;

    private Dictionary<object, Item> Items = new Dictionary<object, Item>();
    private Dictionary<object, Menu> Menus = new Dictionary<object, Menu>();


    [SerializeField]
    private string BasePath = "SceneGame/ScriptableObject/";

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
        }

        LoadScriptable<Item>(ref Items);
        LoadScriptable<Menu>(ref Menus);
    }
    public void LoadScriptable<T>(ref Dictionary<object, T> scriptObject) where T : ScriptableObject
    {
        string path = BasePath + typeof(T).Name;
        T[] Container = Resources.LoadAll<T>(path);

        foreach (var item in Container)
        {
            if (item is IIdentifiable identifiable)
            {
                if (!scriptObject.ContainsKey(identifiable.ID))
                {
                    scriptObject[identifiable.ID] = identifiable as T;
                }
                if (!scriptObject.ContainsKey(identifiable.Name))
                {
                    scriptObject[identifiable.Name] = identifiable as T;
                }
            }


        }

        Debug.Log(scriptObject.Count);
    }
    public Item GetItem(object key)
    {
        if (key is Enum KEnum)
        {
            string enumKey = KEnum.ToString();
            if (Items.TryGetValue(enumKey, out Item item))
            {
                return item;
            }
        }
        else if (Items.TryGetValue(key, out Item item))
        {
            return item;
        }

        Debug.LogError("Don't found Item");
        return null;
    }
}

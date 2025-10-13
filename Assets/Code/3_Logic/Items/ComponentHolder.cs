using System;
using System.Collections.Generic;
using UnityEngine;

public class ComponentHolder : MonoBehaviour
{
    [SerializeField] private ComponentHoldData[] holdData;

    public readonly Dictionary<string, Component> componentMap = new Dictionary<string, Component>();

    private void Start()
    {
        foreach (var component in holdData)
        {
            string name = component.Name;

            if (!componentMap.ContainsKey(name))
            {
                componentMap[name] = component.SpriteRenderer;
            }
        }
    }

    public T GetComponentByName<T>(string Name) where T : Component
    {
        if (componentMap.ContainsKey(Name))
        {
            if (componentMap[Name] is T component)
            {
                return component;
            }
        }
        return null;
    }
}

[Serializable]
public class ComponentHoldData
{
    public string Name;
    public SpriteRenderer SpriteRenderer;
    public Canvas Canvas;
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DIContainerBase : IDiContain
{
    protected readonly Dictionary<Type, object> Scripts;

    public DIContainerBase()
    {
        Scripts = new Dictionary<Type, object>();
    }
    protected DIContainerBase(Dictionary<Type, object> scripts)
    {
        Scripts = scripts ?? new Dictionary<Type, object>();
    }

    
    public bool TryGet<T>(out T result)
    {
        if (Scripts.TryGetValue(typeof(T), out var obj) && obj is T value)
        {
            result = value;
            return true;
        }
        result = default;
        return false;
    }
    public T GetScript<T>() where T : class
    {
        if(Scripts.TryGetValue(typeof(T),out var obj) && obj is T value)
        {
            return value;
        }

        Debug.LogWarning($"[DIContainer] {typeof(T).Name} ไม่ถูก Register!");
        return null;
    }

    public void Register<T>(T script) => Scripts[typeof(T)] = script;
    public void Unregister<T>() => Scripts.Remove(typeof(T));
}

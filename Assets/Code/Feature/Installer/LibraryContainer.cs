using System;
using System.Collections.Generic;
using UnityEngine;

public class LibraryContainer : MonoBehaviour
{
    [SerializeField] ScriptableObject[] scriptableObjects;

    private Dictionary<Type, ILibrary> _library = new();

    private void Awake()
    {
        foreach (ILibrary obj in scriptableObjects)
        {
            _library[obj.GetType()] = obj;
        }
    }

    public ILibrary Get<T>() 
    {
        if (_library.TryGetValue(typeof(T), out ILibrary obj))
        {
            return obj;
        }

        return null;
    }
}
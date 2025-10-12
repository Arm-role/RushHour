using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static readonly Dictionary<Type, IEventBase> _events = new();

    public static void Subscribe<T>(Action<T> listener, object owner = null) where T : struct
    {
        Type type = typeof(T);

        if (!_events.TryGetValue(type, out var eventBase))
        {
            eventBase = new GameEvent<T>();
            _events[type] = eventBase;
        }

        (eventBase as GameEvent<T>)?.Subscribe(listener, owner);
    }

    public static void Unsubscribe<T>(Action<T> listener) where T : struct
    {
        //Debug.Log(typeof(T));

        if (_events.TryGetValue(typeof(T), out var eventBase))
        {
            (eventBase as GameEvent<T>)?.Unsubscribe(listener);
        }
    }
    public static void Invoke<T>(T eventData) where T : struct
    {
        if (_events.TryGetValue(typeof(T), out var eventBase))
        {
            (eventBase as GameEvent<T>)?.Invoke(eventData);
        }
    }
}

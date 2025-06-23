using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventBase
{
    private readonly List<WeakListener> _listeners = new();

    public void Subscribe(Action callback, object owner = null)
    {
        if (callback == null) return;

        var listener = new WeakListener(callback, owner);
        if (!_listeners.Exists(w => w.Equals(listener)))
            _listeners.Add(listener);
    }
    public void UnSubscribe(Action callback)
    {
        _listeners.RemoveAll(w => w.Matches(callback));
    }
    public void UnSubscribeAll(object owner)
    {
        _listeners.RemoveAll(w => w.Owner == owner);
    }
    public virtual void Invoke()
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            var listener = _listeners[i];
            if (!listener.IsAlive)
            {
                _listeners.RemoveAt(i);
                continue;
            }
            listener.Invoke();
        }
    }
    public void DebugListeners()
    {
        Debug.Log($"[EventBase] Active Listeners: {_listeners.Count}");

        foreach (var listener in _listeners)
        {
            Debug.Log(listener.ToString());
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventBase<T>
{
    private readonly List<WeakListener<T>> _listeners = new();

    public void Subscribe(Action<T> callback, object owner = null)
    {
        if (callback == null) { Debug.Log("Action Null"); return; }

        var listener = new WeakListener<T>(callback, owner);
        if (!_listeners.Exists(w => w.Equals(listener)))
            _listeners.Add(listener);
    }
    public void UnSubscribe(Action<T> callback)
    {
        _listeners.RemoveAll(w => w.Matches(callback));
    }
    public void UnSubscribeAll(object owner)
    {
        _listeners.RemoveAll(w => w.Owner == owner);
    }
    public virtual void Invoke(T param)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            var listener = _listeners[i];
            if (!listener.IsAlive)
            {
                _listeners.RemoveAt(i);
                continue;
            }
            listener.Invoke(param);
        }
    }
    public void DebugListeners()
    {
        Debug.Log($"[EventBase<{typeof(T).Name}>] Active Listeners: {_listeners.Count}");

        foreach (var listener in _listeners)
        {
            Debug.Log(listener.ToString());
        }
    }
}

public abstract class EventBaseParam<T> : EventBase<T>
{
    protected T _param = default;
    public override void Invoke(T param)
    {
        base.Invoke(param); _param = param;
    }
    public T GetParamiter() { return _param; }
}
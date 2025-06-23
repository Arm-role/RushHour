using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventBase<T1, T2, T3>
{
    private readonly List<WeakListener<(T1, T2, T3)>> _listeners = new();

    public void Subscribe(Action<T1, T2, T3> callback, object owner = null)
    {
        if (callback == null) { Debug.Log("Action Null"); return; }

        Action<(T1, T2, T3)> wrapper = tuple => callback(tuple.Item1, tuple.Item2, tuple.Item3);

        var listener = new WeakListener<(T1, T2, T3)>(wrapper, owner);

        if (!_listeners.Exists(w => w.Equals(listener)))
            _listeners.Add(listener);
    }
    public void UnSubscribe(Action<T1, T2, T3> callback)
    {
        _listeners.RemoveAll(w =>
        {
            return w.Matches((tuple) => callback(tuple.Item1, tuple.Item2, tuple.Item3));
        });
    }
    public virtual void Invoke(T1 param1, T2 param2, T3 param3)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            var listener = _listeners[i];
            if (!listener.IsAlive)
            {
                _listeners.RemoveAt(i);
                continue;
            }
            listener.Invoke((param1, param2, param3));
        }
    }
    public void DebugListeners()
    {
        Debug.Log($"[EventBase<{typeof(T1).Name}, {typeof(T2).Name}>] Active Listeners: {_listeners.Count}");

        foreach (var listener in _listeners)
        {
            Debug.Log(listener.ToString());
        }
    }
}

public abstract class EventBaseParam<T1, T2, T3> : EventBase<T1, T2, T3>
{
    protected T1 _param1 = default;
    protected T2 _param2 = default;

    public override void Invoke(T1 param1, T2 param2, T3 param3)
    {
        base.Invoke(param1, param2, param3);
        _param1 = param1;
        _param2 = param2;
    }
    public (T1 param1, T2 param2) GetParamiter() { return (_param1, _param2); }
}
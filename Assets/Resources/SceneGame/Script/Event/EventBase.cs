using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventBase<T> : IEvent<T>
{
    private readonly List<Action<T>> _listeners = new List<Action<T>>();

    public void Subscribe(Action<T> action)
    {
        if (!_listeners.Contains(action))
            _listeners.Add(action);
    }
    public void UnSubscribe(Action<T> action)
    {
        if (_listeners.Contains(action))
            _listeners.Remove(action);
    }
    public virtual void Invoke(T param)
    {
        if(_listeners.Count == 0) return;

        foreach (var listener in _listeners.ToArray())
        {
            listener.Invoke(param);
        }
    }
    public void DebugListeners()
    {
        Debug.Log("Debugging Listeners:");

        for (int i = 0; i < _listeners.Count; i++)
        {
            if (_listeners[i] == null)
            {
                Debug.LogWarning($"Listener at index {i} is NULL!");
            }
            else
            {
                Debug.Log($"Listener at index {i}: {_listeners[i].Method.Name} in {_listeners[i].Method.DeclaringType}");
            }
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

public abstract class EventBase : IEvent
{
    private readonly List<Action> _listeners = new List<Action>();

    public void Subscribe(Action action)
    {
        if (!_listeners.Contains(action))
            _listeners.Add(action);
    }
    public void UnSubscribe(Action action)
    {
        if (_listeners.Contains(action))
            _listeners.Remove(action);
    }
    public virtual void Invoke()
    {
        if (_listeners.Count == 0) return;

        foreach (var listener in _listeners.ToArray())
        {
            listener.Invoke();
        }
    }
    public void DebugListeners()
    {
        Debug.Log("Debugging Listeners:");

        for (int i = 0; i < _listeners.Count; i++)
        {
            if (_listeners[i] == null)
            {
                Debug.LogWarning($"Listener at index {i} is NULL!");
            }
            else
            {
                Debug.Log($"Listener at index {i}: {_listeners[i].Method.Name} in {_listeners[i].Method.DeclaringType}");
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;

public class GameEvent<T> : IEventBase
{
    private readonly List<WeakListener<T>> _listener = new();
    public void Subscribe(Action<T> callback, object owner = null)
    {
        var listener = new WeakListener<T>(callback, owner);
        if (!_listener.Exists(w => w.Equals(listener)))
        {
            _listener.Add(listener);
        }

    }
    public void Unsubscribe(Action<T> callback)
    {
        _listener.RemoveAll(w => w.Matches(callback));
    }
    public void UnsubscribeAll(object owner)
    {
        _listener.RemoveAll(w => w.Owner == owner);
    }
    public void Invoke(T eventData)
    {
        for (int i = _listener.Count - 1; i >= 0; i--)
        {
            var listener = _listener[i];
            if (listener.IsAlive)
            {
                listener.Invoke(eventData);
            }
            else
            {
                _listener.RemoveAt(i);
            }
        }
    }
}
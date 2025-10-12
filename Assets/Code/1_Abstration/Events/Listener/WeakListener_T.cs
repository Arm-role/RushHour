using System;
using System.Collections;
using UnityEngine;

public class WeakListener<T>
{
    private readonly WeakReference _targetRef;
    private readonly Action<T> _action;
    public object Owner { get; }

    public bool IsAlive => _targetRef.IsAlive;

    public WeakListener(Action<T> action, object owner = null)
    {
        _action = action;
        _targetRef = new WeakReference(action.Target);
        Owner = owner;
    }

    public void Invoke(T param)
    {
        if (IsAlive)
        {
            _action.Invoke(param);
        }
    }

    public bool Matches(Action<T> action) => _action == action;

    public override bool Equals(object obj)
    {
        return obj is WeakListener<T> other &&
               Equals(other.Owner, Owner) &&
               Equals(other._action, _action);
    }
    public override int GetHashCode()
    {
        return _action.GetHashCode();
    }
    public override string ToString()
    {
        return IsAlive
            ? $"{_action.Method.Name} from {_action.Method.DeclaringType}"
            : "[Dead Listener]";
    }
}
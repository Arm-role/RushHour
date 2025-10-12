using System;
using System.Collections;
using UnityEngine;
public class WeakListener
{
    private readonly WeakReference _targetRef;
    private readonly Action _action;
    public object Owner { get; }

    public bool IsAlive => _targetRef.IsAlive;

    public WeakListener(Action action, object owner = null)
    {
        _action = action;
        _targetRef = new WeakReference(action.Target);
        Owner = owner;
    }

    public void Invoke()
    {
        if (IsAlive)
        {
            _action.Invoke();
        }
    }

    public bool Matches(Action action) => _action == action;

    public override bool Equals(object obj)
    {
        return obj is WeakListener other && other.Owner == Owner;
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

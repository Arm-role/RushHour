using GameEvents;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestroyService
{
    public event Action OnRequestDestruction;

    private readonly HashSet<Action> _handlers = new();

    public ItemDestroyService()
    {
        EventManager.Subscribe<GameFlow>(RequestDestructionAll);
    }

    public void Register(Action callback)
    {
        if (callback == null || _handlers.Contains(callback))
            return;

        OnRequestDestruction += callback;
        _handlers.Add(callback);
    }

    public void Unregister(Action callback)
    {
        if (callback == null || !_handlers.Contains(callback))
            return;

        OnRequestDestruction -= callback;
        _handlers.Remove(callback);
    }

    public void ClearAll()
    {
        foreach (var h in _handlers)
            OnRequestDestruction -= h;

        _handlers.Clear();
    }

    private void RequestDestructionAll(GameFlow evt)
    {
        if(evt.Flow != EGameFlow.GamePlay)
        {
            OnRequestDestruction?.Invoke();
            ClearAll();
        }
    }
}
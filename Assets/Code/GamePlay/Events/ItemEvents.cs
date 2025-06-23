using System;
using UnityEngine;

public class ItemEvents 
{
    public TimeOutEvent OnTimeOut { get; private set; } = new();
    public ItemEjected OnItemEjected { get; internal set; } = new();
    public ItemIdEjected OnItemIdEjected { get; private set; } = new();
    public ItemNameEjected OnItemNameEjected { get; private set; } = new();
    public ItemTransported OnItemTransported { get; private set; } = new();
    public ItemsTransported OnItemsTransported { get; private set; } = new();
}

public class TimeOutEvent : EventBase { }
public class ItemEjected : EventBase<FoodData, Vector2> { }
public sealed class ItemIdEjected : EventBase<int> { }
public sealed class ItemNameEjected : EventBase<string> { }
public sealed class ItemTransported : EventBase<PlayerNetwork, PlayerNetwork, int> { }
public sealed class ItemsTransported : EventBase<PlayerNetwork, PlayerNetwork, int[]> { }
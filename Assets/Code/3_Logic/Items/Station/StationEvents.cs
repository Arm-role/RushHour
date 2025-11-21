using System;

public readonly struct WorkStarted
{
    public readonly Station Station;

    public WorkStarted(Station station)
    {
        Station = station;
    }
}

public readonly struct WorkProgress
{
    public readonly Station Station;

    public WorkProgress(Station station)
    {
        Station = station;
    }
}

public readonly struct WorkCompleted
{
    public readonly Station Station;

    public WorkCompleted(Station station)
    {
        Station = station;
    }
}

public readonly struct WorkCanceled
{
    public readonly Station Station;

    public WorkCanceled(Station station)
    {
        Station = station;
    }
}
public readonly struct ItemAddToStation
{
    public readonly Station Station;
    public readonly Item Item;

    public ItemAddToStation(Station station, Item item)
    {
        Station = station;
        Item = item;
    }
}
public readonly struct OrderSpawnPlate
{
    public readonly Station Station;

    public OrderSpawnPlate(Station station)
    {
        Station = station;
    }
}

public readonly struct ItemRemoveFromStation
{
    public readonly Station Station;
    public readonly Item Item;

    public ItemRemoveFromStation(Station station, Item item)
    {
        Station = station;
        Item = item;
    }
}
public readonly struct IngredienAddToOrder
{
    public readonly Station Station;
    public readonly (int, Item) Ingredient;

    public IngredienAddToOrder(Station station,(int, Item) ingredient)
    {
        Station = station;
        Ingredient = ingredient;
    }
}
public readonly struct PlateServeToOrder
{
    public readonly Station Station;

    public PlateServeToOrder(Station station)
    {
        Station = station;
    }
}
public readonly struct SentMenu
{
    public readonly int MenuId;
    public readonly float ScoreValue;

    public SentMenu(int menuId, float score)
    {
        MenuId = menuId;
        ScoreValue = score;
    }
}
public readonly struct OrderExpiredEvent
{
    public readonly int MenuId;
    public OrderExpiredEvent(int menuId)
    {
        MenuId = menuId;
    }
}

public readonly struct SpawnOrderAgain
{
    public readonly InteractableItem Interactable;

    public SpawnOrderAgain(InteractableItem interactable)
    {
        Interactable = interactable;
    }
}
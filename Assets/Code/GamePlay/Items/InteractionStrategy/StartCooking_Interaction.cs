
using UnityEngine;

public class StartCooking_Interaction : IInteractionStrategy
{
    public bool CanExecute(InteractableItem source, Station targetStation)
    {
        return source.Item is FoodData foodData && !targetStation.stationData.IsWorking && foodData.CanCook(EToolType.Fried);
    }

    public async void Execute(InteractableItem source, Station targetStation)
    {
        FoodData food = source.Item as FoodData;

        var cookState = food.ToolState[EToolType.Fried];
        Item cookItem = await targetStation.CacheItem.Get(cookState.CookItem);
        var stationEvents = targetStation.StationEvent;

        var itemEvents = targetStation.ItemEvents;
        var cookingWork = new TimeCooking_Work(cookState.Timer, food, (FoodData)cookItem, stationEvents, itemEvents);

        targetStation.worker.BeginWork(cookingWork, targetStation);
    }
}
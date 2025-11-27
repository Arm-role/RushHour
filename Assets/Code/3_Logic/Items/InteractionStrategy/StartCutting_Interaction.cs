using System.Threading.Tasks;
using UnityEngine;

public class StartCutting_Interaction : IInteractionStrategy
{
    public int GetExecutionPriority(InteractableItem source, Station targetStation)
    {
        if (source.Item is FoodData foodData && !targetStation.GetData<ToolWorkData>().IsWorking && foodData.CanCook(EToolType.Cutted))
        {
            return 100;
        }
        return 0;
    }
    public async Task<bool> Execute(InteractableItem source, Station targetStation)
    {
        FoodData food = source.Item as FoodData;

        var cookState = food.ToolState[EToolType.Cutted];
        Item cookItem = await targetStation.CacheItem.Get(cookState.CookItem);
        var cookingWork = new RepetitiveAction_Work(cookState.Timer, food, (FoodData)cookItem);

        targetStation.worker.BeginWork(cookingWork, targetStation);

        return true;
    }
}

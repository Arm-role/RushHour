using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "StartCooking_Interaction", menuName = "InteractionStrategy/StartCooking_Interaction")]
public class StartCooking_Interaction : InteractionStrategy
{
    public override int GetExecutionPriority(InteractableItem source, Station targetStation)
    {
        if (source.Item is FoodData foodData && !targetStation.GetData<ToolWorkData>().IsWorking && foodData.CanCook(EToolType.Fried))
        {
            return 100;
        }
        return 0;
    }
    public override async Task<bool> Execute(InteractableItem source, Station targetStation)
    {
        FoodData food = source.Item as FoodData;

        var cookState = food.ToolState[EToolType.Fried];
        Item cookItem = await targetStation.CacheItem.Get(cookState.CookItem);
        var cookingWork = new TimeCooking_Work(cookState.Timer, food, (FoodData)cookItem);

        targetStation.worker.BeginWork(cookingWork, targetStation);

        return true;
    }
}

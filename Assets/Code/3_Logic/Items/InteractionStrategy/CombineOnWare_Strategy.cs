using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class CombineOnWare_Strategy : IInteractionStrategy
{
    public int GetExecutionPriority(InteractableItem source, Station targetStation)
    {
        if (source.Item is FoodData && targetStation.TryGetData<ItemContainerData>(out var containerData))
        {
            if (containerData.FoodRequest.Contains(source.Item.Name)) return 100;
        }
        return 0;
    }
    public Task<bool> Execute(InteractableItem source, Station targetStation)
    {
        FoodData food = source.Item as FoodData;

        targetStation.GetData<ItemContainerData>().FoodContainer.Push(food);
        var itemAddToStation = new ItemAddToStation(targetStation, food);

        EventManager.Invoke(itemAddToStation);
        return Task.FromResult(true);
    }
}
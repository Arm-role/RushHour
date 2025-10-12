using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "CombineOnWare_Strategy", menuName = "InteractionStrategy/CombineOnWare_Strategy")]
public class CombineOnWare_Strategy : InteractionStrategy
{
    public override int GetExecutionPriority(InteractableItem source, Station targetStation)
    {
        if (source.Item is FoodData)
        {
            return 100;
        }
        return 0;
    }
    public override Task<bool> Execute(InteractableItem source, Station targetStation)
    {
        FoodData food = source.Item as FoodData;

        targetStation.GetData<ItemContainerData>().FoodContainer.Push(food);
        var itemAddToStation = new ItemAddToStation(targetStation, food);

        EventManager.Invoke(itemAddToStation);
        return Task.FromResult(true);
    }
}
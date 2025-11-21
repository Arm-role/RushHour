using UnityEngine;
using ItemEvents;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "RemoveItemOnWare_Strategy", menuName = "InteractionStrategy/RemoveItemOnWare_Strategy")]
public class RemoveItemOnWare_Strategy : InteractionStrategy
{
    public override int GetExecutionPriority(InteractableItem source, Station targetStation)
    {
        //if (source == null && targetStation.GetData<ItemContainerData>().FoodContainer.Count > 0)
        //{
        //    return 100;
        //}
        return 0;
    }
    public override Task<bool> Execute(InteractableItem source, Station targetStation)
    {
        var food = targetStation.GetData<ItemContainerData>().FoodContainer.Pop();
        var itemEject = new ItemEjected(food.Name, targetStation.transform.position);
        var itemRemoveFromStation = new ItemRemoveFromStation(targetStation, food);

        EventManager.Invoke(itemEject);
        EventManager.Invoke(itemRemoveFromStation);

        return Task.FromResult(true);
    }
}

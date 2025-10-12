using System;
using System.Collections.Generic;
using System.Threading.Tasks;
public class OrderSpawner
{
    private ItemSpawnManager _itemManager;
    public event Action<InteractableItem, List<Item>> OnOrderSpawnedAndReady;

    public OrderSpawner(ItemSpawnManager itemManager)
    {
        _itemManager = itemManager;
    }
    public async Task CreateOrder(Menu menu)
    {
        InteractableItem interactableItem = await _itemManager.SpawnItemLaunch("Order");

        if (interactableItem.TryGetComponent<Station>(out var station))
        {
            var data = station.GetData<OrderLifecycleManager>();
            var requirementData = new OrderRequirementData();

            requirementData.MenuId = menu.ID;
            requirementData.RequestItems = menu.FoodRequest;
            requirementData.RequiredItemAndCounts = menu.FoodAndCountRequest;
            requirementData.TimeLimit = menu.TimeLimit;
            requirementData.ScoreValue = menu.Score;

            data.Initialize(station, requirementData);
            await station.Interact();
        }
        OnOrderSpawnedAndReady?.Invoke(interactableItem, menu.FoodRequest);
    }
}

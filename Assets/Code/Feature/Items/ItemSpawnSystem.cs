using UnityEngine;

public class ItemSpawnSystem
{
    private readonly ItemManager _itemManager;
    private readonly ItemEvents _itemEvents;

    public ItemSpawnSystem(ItemManager itemManager, ItemEvents itemEvents)
    {
        _itemManager = itemManager;
        _itemEvents = itemEvents;

        _itemEvents.OnItemEjected.Subscribe(GetItemForSpawn);
        _itemEvents.OnItemNameEjected.Subscribe(GetItemNameForSpawn);
        _itemEvents.OnItemIdEjected.Subscribe(GetItemIdForSpawn);
    }

    private async void GetItemForSpawn(FoodData food, Vector2 position)
    {
        Debug.Log(food != null);

        string foodName = food.Name;
        await _itemManager.SpawnItem(foodName, new Vector3(position.x, position.y, 0));
    }
    private async void GetItemNameForSpawn(string name)
    {
        await _itemManager.SpawnItemLaunch(name);
    }
    private async void GetItemIdForSpawn(int id)
    {
        await _itemManager.SpawnItemLaunch(id);
    }
}
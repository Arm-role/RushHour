using ItemEvents;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnSystem
{
    private readonly ItemSpawnHandle _itemManager;

    public ItemSpawnSystem(ItemSpawnHandle itemManager)
    {
        _itemManager = itemManager;

        EventManager.Subscribe<ItemEjected>(GetItemForSpawn);
        EventManager.Subscribe<ItemIdEjected>(GetItemForSpawn);
        EventManager.Subscribe<ItemIdsEjectedLaunch>(GetItemIdsForSpawn);
        EventManager.Subscribe<ItemIdEjectedLaunch>(GetItemIdForSpawn);
        EventManager.Subscribe<ItemNameEjectedLaunch>(GetItemNameForSpawn);
        EventManager.Subscribe<ItemSpawnRequested>(SpawnItemSpawnRequested);

    }
    
    private async void GetItemForSpawn(ItemEjected evt)
    {
        await _itemManager.SpawnItem(evt.ItemName, new Vector3(evt.Position.x, evt.Position.y, 0));
    }
    private async void GetItemForSpawn(ItemIdEjected evt)
    {
        await _itemManager.SpawnItem(evt.ItemId, new Vector3(evt.Position.x, evt.Position.y, 0));
    }
    private async void GetItemIdsForSpawn(ItemIdsEjectedLaunch evt)
    {
        foreach (int id in evt.ItemId)
        {
            await _itemManager.SpawnItemLaunch(id);
        }
    }

    private async void GetItemIdForSpawn(ItemIdEjectedLaunch evt)
    {
        await _itemManager.SpawnItemLaunch(evt.ItemId);
    }
    private async void GetItemNameForSpawn(ItemNameEjectedLaunch evt)
    {
        await _itemManager.SpawnItemLaunch(evt.ItemName);
    }
    private void SpawnItemSpawnRequested(ItemSpawnRequested requested)
    {
        _itemManager.SpawnItemSpawnRequested(requested);
    }
}
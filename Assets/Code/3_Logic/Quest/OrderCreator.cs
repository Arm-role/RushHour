using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
public class OrderCreator
{
    private AssetProvider<Item> _itemCache;

    public event Action<(Item, int), int> OnOrderSpawn;
    public event Action<List<Item>> OnItemSpawn;

    public OrderCreator(AssetProvider<Item> itemCache)
    {
        _itemCache = itemCache;
    }

    public async Task CreateOrder(int playerID, Menu menu)
    {
        Item orderItem = await _itemCache.Get("Order");
        OnOrderSpawn?.Invoke((orderItem, menu.ID), playerID);

        var foodSpawn = menu.FoodSpanw;
        var oterSpawn = menu.OtherFoodSpawn;

        var combineFood = foodSpawn.Concat(oterSpawn).ToList();

        foreach (var item in combineFood)
        {
            Debug.Log(item.Name + "OrderCreator--------------------");
        }

        OnItemSpawn?.Invoke(combineFood);
    }
}

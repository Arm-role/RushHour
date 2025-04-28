using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : SingletonLazy<SpawnManager>
{
    public Dictionary<Type, object> DI_Spawnner = new Dictionary<Type, object>();

    public InputSpawner InputSpawner;
    public ItemCreator ItemCreator = new ItemCreator();

    protected override void OnAwake()
    {
        ItemEvents.Instance.OnSentItem.Subscribe(SpawnItem);
    }

    public void RegisterDI<T>(object ob) where T : class
    {
        DI_Spawnner[typeof(T)] = ob;
    }
    public T GetSriptFromDI<T>() where T : class
    {
        return DI_Spawnner[typeof(T)] as T;
    }

    public void SpawnItem(Item item) => OnSpawnItem(item);

    public GameObject OnSpawnItem(Item item)
    {
        return ItemCreator.Spawn(item, InputSpawner.ForcePower, InputSpawner.SpawnPoint);
    }
    public GameObject OnSpawnItem(Item item, Vector2 position)
    {
        return ItemCreator.Spawn(item, position);
    }


    //public void SpawnItems(IEnumerable<ItemAndCount> listItem)
    //{
    //    foreach (var item in listItem)
    //    {

    //        Enumerable.Range(0, item.count).ToList().ForEach(_ =>
    //        {
    //            OnSpawnItem(item.item);
    //        });
    //    }
    //}


    //public void SortItemFromMenu(Menu menu)
    //{
    //    IEnumerable<ItemAndCount> CombinedList = menu.FoodSpanw.Union(menu.OtherFoodSpawn);
    //    SpawnItems(CombinedList);
    //}
    //public void ConvertToIEnumerable(ItemAndCount[] items)
    //{
    //    IEnumerable<ItemAndCount> CombinedList = items;
    //    SpawnItems(CombinedList);
    //}
}

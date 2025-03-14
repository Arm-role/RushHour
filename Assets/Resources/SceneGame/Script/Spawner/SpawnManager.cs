using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpawnManager
{
    private static SpawnManager instance;
    public static SpawnManager Instance => instance ??= new SpawnManager();

    public Dictionary<Type, object> DI_Spawnner = new Dictionary<Type, object>();

    public InputSpawner _InputSpawner => GetSriptFromDI<InputSpawner>();
    public ItemCreator _ItemCreator = new ItemCreator();

    private bool IsRandom { get; set; } = false;
    private SpawnManager()
    {
        //EvenManager.SentMenu += SortItemFromMenu;
        EvenManager.SentItem += SpawnItem;
    }


    public void RegisterDI<T>(object ob) where T : class
    {
        DI_Spawnner[typeof(T)] = ob;
    }
    public T GetSriptFromDI<T>() where T : class
    {
        return DI_Spawnner[typeof(T)] as T;
    }


    public void SpawnItem(Item item)
    {
        OnSpawnItem(item);
    }
    public GameObject OnSpawnItem(Item item)
    {
        return _ItemCreator.Spawn(item, _InputSpawner.ForcePower, _InputSpawner.SpawnPoint);
    }
    public GameObject OnSpawnItem(Item item, Vector2 position)
    {
        return _ItemCreator.Spawn(item, position);
    }
    public void SpawnItems(IEnumerable<ItemAndCount> listItem)
    {
        foreach (var item in listItem)
        {

            Enumerable.Range(0, item.count).ToList().ForEach(_ =>
            {
                OnSpawnItem(item.item);
            });
        }
    }
    public void SortItemFromMenu(Menu menu)
    {
        IEnumerable<ItemAndCount> CombinedList = menu.FoodSpanw.Union(menu.OtherFoodSpawn);
        SpawnItems(CombinedList);
    }
    public void ConvertToIEnumerable(ItemAndCount[] items)
    {
        IEnumerable<ItemAndCount> CombinedList = items;
        SpawnItems(CombinedList);
    }
}

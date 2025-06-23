using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMenu", menuName = "Food/Menu")]
public class Menu : ScriptableObject, IIdentifiable
{
    [SerializeField] private int id = -1;
    [SerializeField] private string _name;
    public int ID => id;
    public string Name => _name;

    public ItemAndCount[] _FoodRequest;
    public ItemAndCount[] _FoodSpanw;
    public ItemAndCount[] _OtherFoodSpawn;

    public List<Item> FoodRequest => FlattenMenu(_FoodRequest);
    public List<Item> FoodSpanw => FlattenMenu(_FoodSpanw);
    public List<Item> OtherFoodSpawn => FlattenMenu(_OtherFoodSpawn);

    public int Score;

    private void OnValidate()
    {
        _name = name;
    }
    private List<Item> FlattenMenu(ItemAndCount[] menus)
    {
        List<Item> itemList = new();
        foreach (ItemAndCount itemcount in menus)
        {
            Item item = itemcount.item;
            item.Amount = itemcount.count;

            itemList.Add(item);
        }
        return itemList;
    }
}

[Serializable]
public class ItemAndCount
{
    public Item item;
    public int count = 1;
}
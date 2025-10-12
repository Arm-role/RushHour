using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMenu", menuName = "Food/Menu")]
public class Menu : ScriptableObject
{
    [SerializeField] private int id = -1;
    [SerializeField] private string _name;
    public int ID => id;
    public string Name => _name;

    public ItemAndCount[] _FoodRequest;
    public ItemAndCount[] _FoodSpanw;
    public ItemAndCount[] _OtherFoodSpawn;

    public List<Item> FoodRequest => FlattenMenu(_FoodRequest);
    public List<List<Item>> FoodAndCountRequest => FlattenMenu2(_FoodRequest);

    public List<Item> FoodSpanw => FlattenMenu(_FoodSpanw);
    public List<Item> OtherFoodSpawn => FlattenMenu(_OtherFoodSpawn);

    public int Score;
    public float TimeLimit;

    private void OnValidate()
    {
        _name = name;
    }
    private List<Item> FlattenMenu(ItemAndCount[] menus)
    {
        List<Item> itemList = new();

        foreach (var menu in menus)
        {
            for (int i = 0; i < menu.count; i++)
            {
                itemList.Add(menu.item);
            }
        }

        return itemList;
    }
    private List<List<Item>> FlattenMenu2(ItemAndCount[] menus)
    {
        var result = new List<List<Item>>();

        foreach (var menu in menus)
        {
            var itemList = new List<Item>();

            for (int i = 0; i < menu.count; i++)
            {
                itemList.Add(menu.item);
            }

            result.Add(itemList);
        }

        return result;
    }
}

[Serializable]
public class ItemAndCount
{
    public Item item;
    public int count = 1;
}
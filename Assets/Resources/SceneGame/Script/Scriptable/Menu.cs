using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newMenu", menuName = "Food/Menu")]
public class Menu : ScriptableObject, IIdentifiable
{
    [SerializeField] private int id = -1;
    [SerializeField] private string _name;
    public int ID => id;
    public string Name => _name;

    public ItemAndCount[] FoodRequest;

    public ItemAndCount[] FoodSpanw;
    public ItemAndCount[] OtherFoodSpawn;
    public int Score;

    private void OnValidate()
    {
        _name = name;
    }
}

[Serializable]  
public class ItemAndCount
{
    public Item item;
    public int count = 1;
}
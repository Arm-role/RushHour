using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ItemMockTest : MonoBehaviour
{
    [SerializeField] private bool isSpawnItem = false;
    [SerializeField] private bool isLaunch = false;
    [SerializeField] private AssetReferenceT<FoodData> _foodData;
    private ItemManager _itemManager;

    public string[] listName;
    public async void Initialze(ItemManager itemManager)
    {
        _itemManager = itemManager;

        if (isSpawnItem)
        {
            foreach (var item in listName)
            {
                await _itemManager.SpawnItem(item, transform.position);
            }
        }
        
        if (isLaunch)
        {
            foreach (var item in listName)
            {
                await _itemManager.SpawnItemLaunch(item);
            }
        }
    }

    public void OnTest()
    {
       
    }
}

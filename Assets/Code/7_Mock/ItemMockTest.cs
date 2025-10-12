using UnityEngine;
using GameEvents;

public class ItemMockTest : MonoBehaviour
{
    [SerializeField] private bool isSpawnItem = false;
    [SerializeField] private bool isLaunch = false;
    [SerializeField] private bool isMenu = false;

    [SerializeField] private Menu menu;
    private ItemSpawnManager _itemManager;

    public string[] listName;
    public string itemName1;
    public string itemName2;
    public async void Initialze(ItemSpawnManager itemManager)
    {
        _itemManager = itemManager;
        EventManager.Subscribe<OrderFulfilledEvent>(GetScore);

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
        if (isMenu)
        {
            OnSpawnOrder(menu);
        }
    }
    private void GetScore(OrderFulfilledEvent evt)
    {
        Debug.Log("Score = " + evt.Score);
    }
    public async void OnSpawn1()
    {
        await _itemManager.SpawnItem(itemName1, transform.position);
    }
    public async void OnSpawn2()
    {
        await _itemManager.SpawnItem(itemName2, transform.position);
    }
    public async void OnSpawnOrder(Menu menu)
    {
        InteractableItem interactableItem = await _itemManager.SpawnItem("Order", transform.position);

        if (interactableItem.TryGetComponent<Station>(out var station))
        {
            var data = station.GetData<OrderLifecycleManager>();
            var requirementData = new OrderRequirementData();

            requirementData.RequestItems = menu.FoodRequest;
            requirementData.RequiredItemAndCounts = menu.FoodAndCountRequest;
            requirementData.TimeLimit = menu.TimeLimit;
            requirementData.ScoreValue = menu.Score;

            data.Initialize(station, requirementData);
            await station.Interact();
        }

        foreach (Item item in menu.FoodRequest)
        {
            await _itemManager.SpawnItemLaunch(item.Name);
        }
    }
}

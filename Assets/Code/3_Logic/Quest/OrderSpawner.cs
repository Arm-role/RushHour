using ItemEvents;
using UnityEngine;

public class OrderSpawner : MonoBehaviour
{
    private MenuLibrary _menuLibrary;
    private ItemSpawnHandle _itemSpawner;

    public void Initialize(MenuLibrary menuLibrary, ItemSpawnHandle itemSpawnManager)
    {
        _menuLibrary = menuLibrary;
        _itemSpawner = itemSpawnManager;

        EventManager.Subscribe<OrderIdEjectedAndSetUp>(SpawnOrderAndSetUp);

    }
    private void OnDestroy()
    {
        EventManager.Unsubscribe<OrderIdEjectedAndSetUp>(SpawnOrderAndSetUp);
    }
    private async void SpawnOrderAndSetUp(OrderIdEjectedAndSetUp evt)
    {
        Menu menu = _menuLibrary.Find(evt.MenuID);
        InteractableItem interactableItem = await _itemSpawner.SpawnItemLaunch(evt.OrderID);

        if (interactableItem.TryGetComponent<Station>(out var station))
        {
            var data = station.GetData<OrderLifecycleManager>();
            var requirementData = new OrderRequirementData();

            requirementData.MenuId = menu.ID;
            requirementData.RequestItems = menu.FoodRequest;
            requirementData.RequiredItemAndCounts = menu.FoodAndCountRequest;
            requirementData.TimeLimit = menu.TimeLimit;
            requirementData.ScoreValue = menu.Score;

            data.Initialize(station, requirementData);

            await station.Interact();
        }

        EventManager.Invoke(new SpawnOrderAgain(interactableItem));

        Debug.LogWarning($"SpawnOrder-----------{interactableItem.transform.position}---------{interactableItem.gameObject.name}-------------------");
    }
}
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemInitialzeEvent : ISceneDependencyUpdatable
{
    private readonly AssetProvider<Item> _cacheItem;
    private readonly GameObjectSpawner _objectSpawner;
    private readonly ItemDestroyService _itemDestroyService;
    private readonly ItemWorkService _itemWorkService;

    private readonly ItemSpawnHandle _itemSpawnHandle;

    private ObjectActive[] _objectActives;

    private readonly Dictionary<InteractableItem, List<(Action drag, Action release)>> _subscriptions
        = new();

    public ItemInitialzeEvent(
        AssetProvider<Item> cacheItem,
        GameObjectSpawner objectSpawner,
        ItemDestroyService itemDestroyService,
        ItemWorkService itemWorkService,
        ItemSpawnHandle itemSpawnHandle)
    {
        _cacheItem = cacheItem;
        _objectSpawner = objectSpawner;
        _itemDestroyService = itemDestroyService;
        _itemWorkService = itemWorkService;

        _itemSpawnHandle = itemSpawnHandle;

        _itemSpawnHandle.OnSpawnCompleted += InitialzeItem;

        _itemSpawnHandle.OnSpawnCompleted += SubscribeItem;
        _itemSpawnHandle.OnDespawnCompleted += UnsubscribeItem;
    }


    public void UpdateSceneDependencies(DIContainerBase sceneContainer)
    {
        var configProvider = sceneContainer.GetObject<IObjectActives>();
        if (configProvider != null)
        {
            _objectActives = configProvider.ObjectActives;
            Debug.Log("ItemSpawnManager: Scene dependencies (LauncherConfig) have been updated!");
        }
    }
 
    private void SubscribeItem(InteractableItem interactable)
    {
        if (interactable.TryGetComponent<IDestructible>(out var destructible))
        {
            destructible.OnRequestDestruction += _itemSpawnHandle.Despawn;

        }
    }

    private void UnsubscribeItem(InteractableItem interactable)
    {
        if (interactable.TryGetComponent<IDestructible>(out var destructible))
        {
            destructible.OnRequestDestruction -= _itemSpawnHandle.Despawn;
        }
    }

    private void InitialzeItem(InteractableItem interactable)
    {
        if (_itemDestroyService != null)
        {
            _itemDestroyService.Register(interactable.RequestDestruction);
        }

        if (interactable.TryGetComponent<Station>(out var station))
        {
            switch (interactable.itemType)
            {
                case EItemType.Order:
                    station.Initialze(_cacheItem,
                        new OrderLifecycleManager(),
                        new IInteractionStrategy[] { new OrderServe_Interaction() },
                        new IInteractionStrategy[] { new OrderLifecycle_Interaction() });
                    break;

                case EItemType.Tool:
                    if (station.toolType == EToolType.Fried)
                    {
                        station.Initialze(_cacheItem, new ToolWorkData(),
                            new IInteractionStrategy[] { new StartCooking_Interaction() },
                            new IInteractionStrategy[] { new CancelItemWoking_Strategy() });
                    }
                    else if (station.toolType == EToolType.Cutted)
                    {
                        station.Initialze(_cacheItem, new ToolWorkData(),
                            new IInteractionStrategy[] { new StartCutting_Interaction() },
                            new IInteractionStrategy[] { new CancelItemWoking_Strategy() });
                    }

                    break;

                case EItemType.Ware:
                    station.Initialze(_cacheItem, new ItemContainerData(),
                        new IInteractionStrategy[] { new CombineOnWare_Strategy() },
                        new IInteractionStrategy[] { new RemoveItemOnWare_Strategy() });
                    break;
            }

            _itemDestroyService.Register(station.worker.ForceCancel);
        }


        var orderlayerSystem = interactable.OrderLayerSystem;


        if (interactable.TryGetComponent<ToolView>(out var toolView))
        {
            toolView.Initialze(orderlayerSystem, _objectSpawner);
        }

        if (interactable.TryGetComponent<WareView>(out var plateView))
        {
            plateView.Initialze(orderlayerSystem, _objectSpawner);
        }

        if (interactable.TryGetComponent<OrderView>(out var orderView))
        {
            orderView.Initialze(orderlayerSystem, _objectSpawner, _itemWorkService);
        }

        if (interactable.itemType == EItemType.Food)
        {
            SubscribeAction(interactable, _objectActives);
        }
    }

    private void SubscribeAction(InteractableItem interactable, ObjectActive[] objectActives)
    {
        if (!_subscriptions.ContainsKey(interactable))
            _subscriptions[interactable] = new List<(Action, Action)>();

        foreach (var objectActive in objectActives)
        {
            Action dragHandler = objectActive.Show;
            Action releaseHandler = objectActive.Hide;

            interactable.OnDrag += dragHandler;
            interactable.OnRelease += releaseHandler;

            _subscriptions[interactable].Add((dragHandler, releaseHandler));
        }

        interactable.gameObject.AddComponent<Unsubscriber>().Setup(interactable, this);
    }
    public void UnsubscribeAll(InteractableItem interactable)
    {
        if (_subscriptions.TryGetValue(interactable, out var handlers))
        {
            foreach (var (drag, release) in handlers)
            {
                interactable.OnDrag -= drag;
                interactable.OnRelease -= release;
            }
            _subscriptions.Remove(interactable);
        }
    }
    private class Unsubscriber : MonoBehaviour
    {
        private InteractableItem _item;
        private ItemInitialzeEvent _owner;

        public void Setup(InteractableItem item, ItemInitialzeEvent owner)
        {
            _item = item;
            _owner = owner;
        }

        private void OnDisable()
        {
            _owner.UnsubscribeAll(_item);
            Destroy(this);
        }
    }
}

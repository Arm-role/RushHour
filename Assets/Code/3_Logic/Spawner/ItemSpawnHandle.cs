using System;
using ItemEvents;
using UnityEngine;
using System.Threading.Tasks;

public class ItemSpawnHandle : ISceneDependencyUpdatable
{
    private readonly ItemSpawner _itemSpawner;
    private readonly ItemLauncherService _launcherService;

    private LauncherConfig _launcherConfig;

    public event Action<InteractableItem> OnSpawnCompleted;
    public event Action<InteractableItem> OnDespawnCompleted;

    public ItemSpawnHandle(ItemSpawner itemSpawner, ItemLauncherService launcherService)
    {
        _itemSpawner = itemSpawner;
        _launcherService = launcherService;
    }

    public void UpdateSceneDependencies(DIContainerBase sceneContainer)
    {
        var configProvider = sceneContainer.GetObject<ILauncherConfigProvider>();
        if (configProvider != null)
        {
            _launcherConfig = configProvider.LauncherConfig;
            Debug.Log("ItemSpawnManager: Scene dependencies (LauncherConfig) have been updated!");
        }
    }

    private async Task<InteractableItem> CoreSpawn(Func<Task<InteractableItem>> spawnAction)
    {
        var interactionItem = await spawnAction();
        if (interactionItem.TryGetComponent<IPoolable<InteractableItem>>(out var poolable))
        {
            if (!poolable.IsAlive)
                poolable.OnSpawnFromPool(interactionItem);
            else
            {
                Debug.LogError("-----------Is Alive----------");
            }
                
        }

        OnSpawnCompleted?.Invoke(interactionItem);
        EventManager.Invoke(new ItemSpawned(interactionItem));

        return interactionItem;
    }
    public void Despawn(InteractableItem interaction)
    {
        if (interaction.TryGetComponent<IPoolable<InteractableItem>>(out var poolable))
        {
            if (poolable.IsAlive)
                poolable.OnReturnToPool(interaction);
            else
            {
                Debug.LogError("-----------Don't Alive----------");
            }
        }

        OnDespawnCompleted?.Invoke(interaction);
        _itemSpawner.DespawnItem(interaction);
    }

    public async Task<InteractableItem> SpawnItem(string name, Vector3 position)
    {
        return await CoreSpawn(() => _itemSpawner.SpawnItem(name, position));
    }

    public async Task<InteractableItem> SpawnItem(int id, Vector3 position)
    {
        return await CoreSpawn(() => _itemSpawner.SpawnItem(id, position));
    }

    public async Task<InteractableItem> SpawnItemLaunch(string name)
    {
        float Randomer = UnityEngine.Random.Range(
            _launcherConfig.SpawnPointLeft.position.x,
            _launcherConfig.SpawnPointRight.position.x);

        Vector2 spawnPoint = new Vector2(Randomer, _launcherConfig.SpawnPointLeft.position.y);

        var interactionItem = await CoreSpawn(() => _itemSpawner.SpawnItem(name, spawnPoint));

        if (interactionItem != null)
        {
            _launcherService.Launch(interactionItem.gameObject, _launcherConfig.ForcePower, spawnPoint);
        }

        return interactionItem;
    }

    public async Task<InteractableItem> SpawnItemLaunch(int id)
    {
        float Randomer = UnityEngine.Random.Range(
            _launcherConfig.SpawnPointLeft.position.x,
            _launcherConfig.SpawnPointRight.position.x);

        Vector2 spawnPoint = new Vector2(Randomer, _launcherConfig.SpawnPointLeft.position.y);

        var interactionItem = await CoreSpawn(() => _itemSpawner.SpawnItem(id, spawnPoint));

        if (interactionItem != null)
        {
            _launcherService.Launch(interactionItem.gameObject, _launcherConfig.ForcePower, spawnPoint);
        }

        return interactionItem;
    }

    public async void SpawnItemSpawnRequested(ItemSpawnRequested evt)
    {
        InteractableItem interactable = await CoreSpawn(() => _itemSpawner.SpawnItem(evt.ItemName, evt.Position));
        EventManager.Invoke(new PlateReadyForOrder(evt.OrderId, interactable));
    }
}
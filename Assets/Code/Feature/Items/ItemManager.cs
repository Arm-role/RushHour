using System;
using System.Threading.Tasks;
using UnityEngine;

public class ItemManager
{ 
    private readonly ItemSpawner _itemSpawner;
    private readonly ItemInitialzeEvent _itemInstallEvent;
    private readonly ItemLauncherService _launcherService;

    private LauncherConfig _launcherConfig;

    public ItemManager(ItemSpawner itemSpawner, ItemInitialzeEvent itemInstallEvent, ItemLauncherService launcherService, LauncherConfig launcherConfig)
    {
        _itemSpawner = itemSpawner;
        _itemInstallEvent = itemInstallEvent;
        _launcherService = launcherService;
        _launcherConfig = launcherConfig;
    }

    private async Task<InteractableItem> CoreSpawn(Func<Task<InteractableItem>> spawnAction)
    {
        var interactionItem = await spawnAction();
        interactionItem.OnRequestDestruction += _itemSpawner.DespawnItem;
        _itemInstallEvent.InitialzeItem(interactionItem.gameObject);

        return interactionItem;
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
        int Randomer = UnityEngine.Random.Range(0, _launcherConfig.SpawnPoints.Length);
        Transform spawnPoint = _launcherConfig.SpawnPoints[Randomer];

        var interactionItem = await CoreSpawn(() => _itemSpawner.SpawnItem(name, spawnPoint.position));

        if (interactionItem != null)
        {
            _launcherService.Launch(interactionItem.gameObject, _launcherConfig.ForcePower, spawnPoint);
        }

        return interactionItem;
    }
    public async Task<InteractableItem> SpawnItemLaunch(int id)
    {
        int Randomer = UnityEngine.Random.Range(0, _launcherConfig.SpawnPoints.Length);
        Transform spawnPoint = _launcherConfig.SpawnPoints[Randomer];

        var interactionItem = await CoreSpawn(() => _itemSpawner.SpawnItem(id, spawnPoint.position));

        if (interactionItem != null)
        {
            _launcherService.Launch(interactionItem.gameObject, _launcherConfig.ForcePower, spawnPoint);
        }

        return interactionItem;
    }
}
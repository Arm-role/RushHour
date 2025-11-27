using GameEvents;
using System;
using UnityEngine;

public class GameSceneInstaller : SceneInstaller, ILauncherConfigProvider, IObjectActives
{
    [Header("Library")]
    [SerializeField] private ItemLibrary _itemLibrary;
    [SerializeField] private WorkLibrary _workLibrary;
    [SerializeField] private GameObjectLibrary _gameObjectLibrary;
    [SerializeField] private MenuLibrary _menuLibrary;

    [Header("spawner")]
    [SerializeField] private ParticleLibrary _particleLibrary;
    [SerializeField] private LauncherConfig _launcherConfig;
    [SerializeField] private OrderSpawner _orderSpawner;

    [Header("Network Quest")]
    [SerializeField] private MasterGameController _masterGameController;

    [Header("DragManager")]
    [SerializeField] private ObjectActive[] _objectActives;
    [SerializeField] private TransportItem _transportItem;

    public LauncherConfig LauncherConfig => _launcherConfig;
    public ObjectActive[] ObjectActives => _objectActives;

    private event Action OnDestroyer;
    protected override void Start() => base.Start();

    protected override void Initialzed(DIContainerBase globalContainer)
    {
        AppInstaller.OnServiceReady -= Initialzed;

        var sceneContainer = new DIContainerBase(globalContainer);
        sceneContainer.Register<ILauncherConfigProvider>(this);
        sceneContainer.Register<IObjectActives>(this);

        _workLibrary.Initialize();

        var poolService = sceneContainer.GetObject<IAdressablePoolService>();
        var CacheService = sceneContainer.GetObject<IAdressableCacheService>();

        var particalService = new ParticalService(poolService, _particleLibrary);
        var particleManager = new ParticleManager(particalService);

        var cacheItem = new AssetProvider<Item>(CacheService, _itemLibrary);
        var itemWorkService = new ItemWorkService(cacheItem, _workLibrary);

        var itemDestroyService = new ItemDestroyService();

        var itemSpawnHandle = sceneContainer.GetObject<ItemSpawnHandle>();
        if (itemSpawnHandle == null)
        {
            var itemSpawner = new ItemSpawner(poolService, _itemLibrary);
            var itemLauncherService = new ItemLauncherService();
            itemSpawnHandle = new ItemSpawnHandle(itemSpawner, itemLauncherService);
            globalContainer.Register(itemSpawnHandle);
        }

        itemSpawnHandle.UpdateSceneDependencies(sceneContainer);

        var itemSpawnSystem = new ItemSpawnSystem(itemSpawnHandle);

        var itemInitialzeEvent = sceneContainer.GetObject<ItemInitialzeEvent>();
        if (itemInitialzeEvent == null)
        {
            var objectSpawner = new GameObjectSpawner(poolService, _gameObjectLibrary);

            itemInitialzeEvent = new ItemInitialzeEvent(
                cacheItem, 
                objectSpawner, 
                itemDestroyService, 
                itemWorkService, 
                itemSpawnHandle);

            globalContainer.Register(itemInitialzeEvent);
        }

        itemInitialzeEvent.UpdateSceneDependencies(sceneContainer);

        var spawnListeners = new IOnSpawnListener[] { };
        var despawnListeners = new IOnDespawnListener[] { };

        var spawnBroker = new SpawnEventBroker(itemSpawnHandle, spawnListeners, despawnListeners);


        var orderConnection = new OrderOrchestratorSystem(OnDestroyer);

        _transportItem.Initialze(_itemLibrary);
        _orderSpawner.Initialize(_menuLibrary, itemSpawnHandle);

        _masterGameController.Initialze(sceneContainer.GetObject<GameState>(), cacheItem, itemSpawnHandle);

        globalContainer.Register(particleManager);
        globalContainer.Register(itemDestroyService);

        Destroy(gameObject);
    }

    protected override void OnDestroy()
    {
        OnDestroyer?.Invoke();
        base.OnDestroy();

    }
}

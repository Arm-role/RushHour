using GameEvents;
using System;
using UnityEngine;

public class GameSceneInstaller : SceneInstaller
{
    [Header("Library")]
    [SerializeField] private ItemLibrary _itemLibrary;
    [SerializeField] private WorkLibrary _workLibrary;
    [SerializeField] private GameObjectLibrary _gameObjectLibrary;

    [Header("spawner")]
    [SerializeField] private ParticleLibrary _particleLibrary;
    [SerializeField] private LauncherConfig _launcherConfig;

    [Header("Quest")]
    [SerializeField] private OfflineGameController _offlineGameController;
    [SerializeField] private GameEndController _gameEndController;
    [SerializeField] private GameTimeController _gameTimeController;
    [SerializeField] private PopupController _popupController;

    [Header("DragManager")]
    [SerializeField] private DragManager _dragManager;
    [SerializeField] private ObjectActive[] _objectActives;
    [SerializeField] private TransportItem _transportItem;

    [Header("Mock")]
    [SerializeField] private ItemMockTest _itemMockTest;

    private event Action OnDestroyer;
    protected override void Start() => base.Start();


    protected override void Initialzed(DIContainerBase globalContainer)
    {
        AppInstaller.OnServiceReady -= Initialzed;

        var container = new DIContainerBase(globalContainer);

        _workLibrary.Initialize();
        var gameSessionManager = FindObjectOfType<GameSessionManager>();

        var poolService = container.GetObject<IAdressablePoolService>();
        var CacheService = container.GetObject<IAdressableCacheService>();

        var particalService = new ParticalService(poolService, _particleLibrary);
        var particleManager = new ParticleManager(particalService);

        var cacheItem = new AssetProvider<Item>(CacheService, _itemLibrary);
        var itemWorkService = new ItemWorkService(cacheItem, _workLibrary);

        var objectSpawner = new GameObjectSpawner(poolService, _gameObjectLibrary);
        var itemSpawner = new ItemSpawner(poolService, _itemLibrary);
        var itemDestroyService = new ItemDestroyService(gameSessionManager);

        var itemInitialzeEvent = new ItemInitialzeEvent(cacheItem, objectSpawner, itemDestroyService, itemWorkService, _objectActives);

        var ItemLauncherService = new ItemLauncherService();
        var itemManager = new ItemSpawnManager(itemSpawner, ItemLauncherService, _launcherConfig);
        var itemSpawnSystem = new ItemSpawnSystem(itemManager);

        var orderConnection = new OrderOrchestratorSystem(OnDestroyer);

        container.Register(particleManager);
        container.Register(itemDestroyService);

        _transportItem.Initialze(_itemLibrary);

        _itemMockTest.Initialze(itemManager);

        _offlineGameController.Initialze(itemManager, _gameEndController, _gameTimeController, _popupController);

        GameFlowState.Set(EGameFlow.Start);
        Destroy(gameObject);
    }

    protected override void OnDestroy()
    {
        OnDestroyer?.Invoke();
        base.OnDestroy();

    }
}

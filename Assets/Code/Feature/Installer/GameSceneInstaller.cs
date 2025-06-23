using UnityEngine;

public class GameSceneInstaller : MonoBehaviour
{
    [Header("Library")]
    [SerializeField] private ItemLibrary _itemLibrary;
    [SerializeField] private GameObjectLibrary _gameObjectLibrary;

    [Header("GamePlay Layer")]
    [SerializeField] private ParticleLibrary _particleLibrary;
    [SerializeField] private LauncherConfig _launcherConfig;
    [SerializeField] private TransportItem _transportItem;

    [Header("Feature Layer")]
    [SerializeField] private LibraryContainer _libraryContainer;
    [SerializeField] private DragManager _dragManager;
    [SerializeField] private ItemMockTest _itemMockTest;

    private void Start()
    {
        if (AppInstaller.IsReady)
        {
            Initialzed(AppInstaller.Container);
        }
        else
        {
            AppInstaller.OnServiceReady += Initialzed;
        }
    }
    private void OnDestroy()
    {
        AppInstaller.OnServiceReady -= Initialzed;
    }
    private void Initialzed(DIContainerBase globalContainer)
    {
        AppInstaller.OnServiceReady -= Initialzed;

        var container = new DIContainerBase(globalContainer);
        var itemEvents = new ItemEvents();

        var playerConfig = container.GetObject<PlayerManager>();
        var poolService = container.GetObject<IAdressablePoolService>();
        var CacheService = container.GetObject<IAdressableCacheService>();

        var particalService = new ParticalService(poolService, _particleLibrary);
        var particleManager = new ParticleManager(particalService);

        var sentItem = new SentItem(itemEvents);
        var orderByplayer = new OrderByItemPlayers(itemEvents);

        var cacheItem = new AssetProvider<Item>(CacheService, _itemLibrary);

        var itemSpawner = new ItemSpawner(poolService, _itemLibrary);
        var itemInitialzeEvent = new ItemInitialzeEvent(cacheItem, itemEvents);
        var ItemLauncherService = new ItemLauncherService();
        var itemManager = new ItemManager(itemSpawner, itemInitialzeEvent, ItemLauncherService, _launcherConfig);
        var itemSpawnSystem = new ItemSpawnSystem(itemManager, itemEvents);

        container.Register(particleManager);
        container.Register(itemManager);

        _dragManager.Initialze(container.GetObject<ParticleManager>());
        _itemMockTest.Initialze(container.GetObject<ItemManager>());
        //_transportItem.Initialze(_itemLibrary, itemEvents, playerConfig);
        Destroy(gameObject);
    }
}

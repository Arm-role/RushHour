using Fusion;
using System.Linq;
using UnityEngine;
using PlayerEvents;
using System.Collections;
using System.Collections.Generic;

public class MasterGameController : NetworkBehaviour
{
    [Header("Dependencies (Assign in Inspector)")]
    [SerializeField] private GameModeData _gameModeData;
    [SerializeField] private ItemLibrary _itemLibrary;

    private GameState _gameState;

    private AssetProvider<Item> _itemCache;
    private ItemSpawnHandle _itemSpawnManager;

    private OrderCreator _orderSpawner;
    private IQuestService _questService;
    private LevelProgression _levelProgression;
    private IQuestStateProvider _questStateProvider;

    public override void Spawned()
    {
        if (!Runner.IsSharedModeMasterClient)
        {
            enabled = false;
            return;
        }

        _orderSpawner = new OrderCreator(_itemCache);
        _levelProgression = new LevelProgression(_gameModeData);

        _orderSpawner.OnOrderSpawn += SpawnOrder;
        _orderSpawner.OnItemSpawn += SpawnItemMaterials;

        _gameState.OnSentItem += OnPlayerSubmitMenu;
        _gameState.OnPlayerOrderExpired += OnOrderExpired;

        StartNextLevel();
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        _orderSpawner.OnOrderSpawn -= SpawnOrder;
        _orderSpawner.OnItemSpawn -= SpawnItemMaterials;

        _gameState.OnSentItem -= OnPlayerSubmitMenu;
        _gameState.OnPlayerOrderExpired -= OnOrderExpired;
    }
    public void Initialze(GameState gameState, AssetProvider<Item> itemCache, ItemSpawnHandle itemSpawnManager)
    {
        _gameState = gameState;
        _itemCache = itemCache;
        _itemSpawnManager = itemSpawnManager;
    }

    private void StartNextLevel()
    {
        LevelData nextLevel = _levelProgression.AdvanceToNextLevel();
        if (nextLevel != null)
        {
            _questService = new FusionQuestService(_gameState, nextLevel);
            _questStateProvider = (IQuestStateProvider)_questService;
            StartCoroutine(LevelStartFlow(nextLevel));
        }
        else
        {
            _gameState.CurrentPhase = EGameFlow.GameEnd;
        }
    }

    private IEnumerator LevelStartFlow(LevelData levelData)
    {
        _gameState.GlobalLevelIndex = levelData.LevelId;
        _gameState.CurrentRunLevelIndex = _levelProgression.CurrentLevelIndex;
        _gameState.IsRandomLevel = _levelProgression.IsRandomLevel();
        _gameState.MaxGameTime = levelData.MaxGameTime;
        _gameState.GameTimer = levelData.MaxGameTime;
        _gameState.CurrentPhase = EGameFlow.LevelStartPopup;

        float popupTotalTime = 0;

        if (_levelProgression.IsRandomLevel()) popupTotalTime = 5;
        else foreach (var p in levelData.StartPopupSprites) popupTotalTime += p.timer;

        yield return new WaitForSeconds(popupTotalTime);

        var players = PlayerRegistry.Instance.GetAllPlayers().Select(p => p.PlayerRef).ToList();
        _questService.SetupLevel(levelData, players);
        UpdateAllPlayerOrders();

        _gameState.CurrentPhase = EGameFlow.GamePlay;
        SpawnItemTool(_levelProgression.CurrentLevel.ItemHelp);
    }

    private void OnPlayerSubmitMenu(int PlayerId, int MenuId, float ScoreValue)
    {
        var player = PlayerRegistry.Instance.GetPlayer(PlayerId);
        if (player == null) return;

        bool success = _questService.SubmitMenu(player.PlayerRef, MenuId);
        _gameState.TotalScore += ScoreValue;

        if (_questStateProvider.IsLevelComplete)
        {
            StartNextLevel();
        }
        else if (success)
        {
            EventManager.Invoke(new TotalScoreEvent(_gameState.TotalScore));
            UpdatePlayerOrder(player.PlayerRef);
            _gameState.GameTimer = _gameState.MaxGameTime;
        }
        else
        {
            Debug.LogWarning("Submission FAILED!");
        }
    }

    private void OnOrderExpired(int PlayerId)
    {
        if (!Runner.IsSharedModeMasterClient) return;

        var player = PlayerRegistry.Instance.GetPlayer(PlayerId);
        if (player == null) return;

        _questService.AssignNewQuestToPlayer(player.PlayerRef);
        UpdatePlayerOrder(player.PlayerRef);
    }

    private void UpdateAllPlayerOrders()
    {
        var assignments = _questStateProvider.PlayerAssignments;

        foreach (var assignment in assignments)
        {
            UpdatePlayerOrder(assignment.Key);
        }
    }

    private async void UpdatePlayerOrder(PlayerRef player)
    {
        var assignments = _questStateProvider.PlayerAssignments;
        if (assignments.TryGetValue(player, out int menuId) && menuId != -1)
        {
            Menu menuToSpawn = FindMenuById(menuId);
            if (menuToSpawn != null)
            {
                await _orderSpawner.CreateOrder(player.PlayerId, menuToSpawn);
            }
        }
    }

    private void SpawnOrder((Item, int) orderItem, int playerID)
    {
        int orderID = _itemLibrary.FindIdByName(orderItem.Item1.Name);

        byte[] orderIDb = ByteConverter.IntToBytes(orderID);
        byte[] menuIDb = ByteConverter.IntToBytes(orderItem.Item2);

        _gameState.RPC_RequestOrderSpawn(orderIDb, menuIDb, playerID);
    }
    private async void SpawnItemTool(Item[] items)
    {
        var players = PlayerRegistry.Instance.GetAllPlayers();

        Dictionary<PlayerRef, List<Item>> playerItemMap = new Dictionary<PlayerRef, List<Item>>();
        List<Item> availableItems = new List<Item>(items);

        int playerCount = players.Count();
        if (playerCount == 0 || availableItems.Count == 0) return;

        int baseCount = availableItems.Count / playerCount;
        int remainder = availableItems.Count % playerCount;

        List<int> extraReceivers = new List<int>();
        for (int i = 0; i < remainder; i++)
            extraReceivers.Add(i);
        extraReceivers = extraReceivers.OrderBy(_ => UnityEngine.Random.value).ToList();

        int currentIndex = 0;
        foreach (var player in players)
        {
            int giveCount = baseCount;
            if (extraReceivers.Contains(currentIndex)) giveCount++;

            List<Item> assigned = new List<Item>();
            for (int i = 0; i < giveCount && availableItems.Count > 0; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, availableItems.Count);
                assigned.Add(availableItems[randomIndex]);
                availableItems.RemoveAt(randomIndex);
            }

            if (assigned.Count > 0)
                playerItemMap[player.PlayerRef] = assigned;

            currentIndex++;
        }

        foreach (var kvp in playerItemMap)
        {
            PlayerRef target = kvp.Key;
            List<Item> playerItems = kvp.Value;

            int[] itemIds = playerItems.Select(i => _itemLibrary.FindIdByName(i.Name)).ToArray();
            byte[] itemBytes = ByteConverter.IntArrayToBytes(itemIds);

            _gameState.RPC_RequestItemSpawn(itemBytes, target);
        }

        foreach (var leftover in availableItems)
        {
            await _itemSpawnManager.SpawnItemLaunch(leftover.Name);
        }
    }

    private async void SpawnItemMaterials(List<Item> items)
    {
        foreach (var item in items)
        {
            Debug.Log(item.Name + "Master--------------------");
        }

        var players = PlayerRegistry.Instance.GetAllPlayers();

        Dictionary<PlayerRef, List<Item>> playerItemMap = new Dictionary<PlayerRef, List<Item>>();
        List<Item> availableItems = new List<Item>(items);

        foreach (var player in players)
        {
            int count = UnityEngine.Random.Range(0, availableItems.Count());

            List<Item> assigned = new List<Item>();
            for (int i = 0; i < count && availableItems.Count > 0; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, availableItems.Count);
                assigned.Add(availableItems[randomIndex]);
                availableItems.RemoveAt(randomIndex);
            }

            if (assigned.Count > 0)
                playerItemMap[player.PlayerRef] = assigned;
        }

        foreach (var kvp in playerItemMap)
        {
            PlayerRef target = kvp.Key;
            List<Item> playerItems = kvp.Value;

            playerItems.ForEach(i => Debug.Log(i.Name + "----------------"));

            int[] itemIds = playerItems.Select(i => _itemLibrary.FindIdByName(i.Name)).ToArray();
            byte[] itemBytes = ByteConverter.IntArrayToBytes(itemIds);

            _gameState.RPC_RequestItemSpawn(itemBytes, target);
        }

        foreach (var leftover in availableItems)
        {
            await _itemSpawnManager.SpawnItemLaunch(leftover.Name);
        }
    }

    private Menu FindMenuById(int id)
    {
        return _gameModeData.LevelList
            .SelectMany(l => l.MenuList)
            .FirstOrDefault(m => m.ID == id);
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OrderExpired(int targetId)
    {
        if (!Runner.IsSharedModeMasterClient) return;

        var player = PlayerRegistry.Instance.GetPlayer(targetId);
        if (player == null) return;

        _questService.AssignNewQuestToPlayer(player.PlayerRef);
        UpdatePlayerOrder(player.PlayerRef);
    }
}
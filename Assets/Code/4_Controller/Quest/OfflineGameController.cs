using GameEvents;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using PlayerEvents;

public class OfflineGameController : MonoBehaviour
{
    [Header("Dependencies (Assign in Inspector)")]
    [SerializeField] private GameModeData _gameModeData;

    [Header("Game Setup")]
    [SerializeField] private int _numberOfPlayers = 2;
    [SerializeField] private float _totalScore;
    [SerializeField] private float _maxGameTime = 30;
    [SerializeField] private float _gameTimeSpeed = 1;

    private GameEndController _gameEndController;
    private GameTimeController _gameTimeController;
    private PopupController _popupController;

    private ItemSpawnManager _itemSpawner;
    private OrderSpawner _orderSpawner;

    // --- Model Instances (Pure C#) ---
    private QuestManager_Offline _questManager;
    private LevelProgression _levelProgression;

    private List<PlayerOffline> _players = new List<PlayerOffline>();
    private List<Item> _itemMaterials = new List<Item>();

    public void Initialze(ItemSpawnManager itemSpawner, GameEndController gameEndController, GameTimeController gameTimeController, PopupController popupController)
    {
        _itemSpawner = itemSpawner;
        _orderSpawner = new OrderSpawner(_itemSpawner);

        _questManager = new QuestManager_Offline();
        _questManager = new QuestManager_Offline();
        _levelProgression = new LevelProgression(_gameModeData);

        _gameEndController = gameEndController;
        _gameTimeController = gameTimeController;
        _popupController = popupController;

        // Create mock players
        for (int i = 1; i <= _numberOfPlayers; i++)
        {
            _players.Add(new PlayerOffline(i, $"Player {i}"));
        }

        _questManager.OnQuestStateChanged += OnQuestStateChanged;
        _orderSpawner.OnOrderSpawnedAndReady += SpawnItemMaterials;

        _gameTimeController.OnGameTimeFinished += OnEndGame;
        
        _popupController.OnPopupFinished += () => GameFlowState.Set(EGameFlow.Run);

        EventManager.Subscribe<SentMenu>(OnPlayerSubmitMenu);
        EventManager.Subscribe<OrderExpiredEvent>(OnOrderExpired);

        StartNextLevel();
        _gameTimeController.StartGameTime(_totalScore, _maxGameTime, _gameTimeSpeed);
    }

    private void OnEndGame()
    {
        _gameEndController.StartEndSequence(_totalScore);
        EventManager.Invoke(new GameFlow(EGameFlow.End));
    }

    private void OnDestroy()
    {
        if (_questManager != null)
            _questManager.OnQuestStateChanged -= OnQuestStateChanged;
        if (_orderSpawner != null)
            _orderSpawner.OnOrderSpawnedAndReady -= SpawnItemMaterials;

        if (_gameTimeController != null)
            _gameTimeController.OnGameTimeFinished -= OnEndGame;
        if (_popupController != null)
        
            _popupController.OnPopupFinished -= () => GameFlowState.Set(EGameFlow.Run);

        EventManager.Unsubscribe<SentMenu>(OnPlayerSubmitMenu);
        EventManager.Unsubscribe<OrderExpiredEvent>(OnOrderExpired);
    }

    private void StartNextLevel()
    {
        LevelData nextLevel = _levelProgression.AdvanceToNextLevel();
        if (nextLevel != null)
        {
            _popupController.StartPopupSequence(nextLevel.StartPopupSprites);
            _questManager.SetupLevel(nextLevel, _players);

            EventManager.Invoke(new GameFlow(EGameFlow.Pause));
        }
        else
        {
            Debug.Log("GAME COMPLETE! No more levels.");
        }
    }

    private void OnPlayerSubmitMenu(SentMenu evt)
    {
        Debug.Log($"CONTROLLER: Player {evt.PlayerId} submitted Menu {evt.MenuId}. Telling QuestManager...");

        bool success = _questManager.SubmitMenu(evt.PlayerId, evt.MenuId);

        if (success)
        {
            _totalScore += evt.ScoreValue;
            _gameTimeController.StartGameTime(_totalScore, _maxGameTime, _gameTimeSpeed);

            EventManager.Invoke(new TotalScoreEvent(_totalScore));
        }
        else
        {
            Debug.LogWarning("Submission FAILED!");
        }
    }
    private async void UpdatePlayerOrder(int playerId)
    {
        int menuId = _questManager.GetPlayerAssignment(playerId);
        if (menuId != -1)
        {
            Menu menuToSpawn = _gameModeData.LevelList
                .SelectMany(l => l.MenuList)
                .FirstOrDefault(m => m.ID == menuId);

            if (menuToSpawn != null)
            {
                Debug.Log($"Spawning NEW order '{menuToSpawn.Name}' for player {playerId} because their old one expired.");
                await _orderSpawner.CreateOrder(menuToSpawn); // This needs to be player-specific
            }
        }
    }

    private void OnQuestStateChanged()
    {
        if (_questManager.IsLevelComplete)
        {
            Debug.Log("CONTROLLER: QuestManager reports level complete. Starting next level...");
            StartNextLevel();
        }
        else
        {
            Debug.Log("CONTROLLER: Quest state changed. Telling OrderView to update.");
            UpdateAllPlayerOrders();
        }
    }

    private async void UpdateAllPlayerOrders()
    {
        var assignments = _questManager.PlayerAssignments;
        foreach (var assignment in assignments)
        {
            int menuId = assignment.Value;
            if (menuId != -1)
            {
                Menu menuToSpawn = _gameModeData.LevelList
                    .SelectMany(l => l.MenuList)
                    .FirstOrDefault(m => m.ID == menuId);

                if (menuToSpawn != null)
                {
                    await _orderSpawner.CreateOrder(menuToSpawn);
                }
            }
        }
    }

    private async void SpawnItemMaterials(InteractableItem orderInteractable, List<Item> items)
    {
        _itemMaterials = new List<Item>(items);

        orderInteractable.gameObject.SetActive(true);

        foreach (var item in _itemMaterials)
        {
            await _itemSpawner.SpawnItemLaunch(item.Name);
        }
    }

    private void OnOrderExpired(OrderExpiredEvent evt)
    {
        Debug.Log($"CONTROLLER: Order for Menu {evt.MenuId} expired for Player {evt.PlayerId}. Assigning a new quest.");

        _questManager.AssignNewQuestToPlayer(evt.PlayerId);

        UpdatePlayerOrder(evt.PlayerId);
    }
}   
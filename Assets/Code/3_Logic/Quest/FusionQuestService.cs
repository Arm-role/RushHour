using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class FusionQuestService : IQuestService, IQuestStateProvider
{
    private readonly GameState _gameState;
    private readonly LevelData _currentLevelData;

    public FusionQuestService(GameState gameState, LevelData levelData)
    {
        _gameState = gameState;
        _currentLevelData = levelData;
    }

    public int CompletedCount => _gameState.CompletedCount;
    public int RequiredCount => _gameState.RequiredCount;
    public bool IsLevelComplete => CompletedCount >= RequiredCount;

    public IReadOnlyDictionary<PlayerRef, int> PlayerAssignments
    {
        get
        {
            var dict = new Dictionary<PlayerRef, int>();
            foreach (var kvp in _gameState.PlayerAssignments)
            {
                dict[kvp.Key] = kvp.Value.AssignedMenuId;
            }
            return dict;
        }
    }

    public void SetupLevel(LevelData levelData, List<PlayerRef> players)
    {
        if (!_gameState.Object.HasStateAuthority)
            return;

        _gameState.RequiredCount = levelData.RequestCount;
        _gameState.CompletedCount = 0;
        _gameState.PlayerAssignments.Clear();

        // เพิ่มผู้เล่นทั้งหมดเข้า dictionary
        foreach (var player in players)
        {
            _gameState.PlayerAssignments.Add(player, new PlayerCurrentQuest
            {
                PlayerRef = player,
                AssignedMenuId = -1
            });

            AssignNewQuestToPlayer(player);
        }
    }

    public bool SubmitMenu(PlayerRef playerId, int menuId)
    {
        if (!_gameState.Object.HasStateAuthority)
            return false;

        if (!_gameState.PlayerAssignments.ContainsKey(playerId))
            return false;

        var quest = _gameState.PlayerAssignments[playerId];

        if (quest.AssignedMenuId != menuId)
            return false;

        _gameState.CompletedCount++;

        AssignNewQuestToPlayer(playerId);

        return true;
    }

    public void AssignNewQuestToPlayer(PlayerRef playerId)
    {
        if (!_gameState.Object.HasStateAuthority || _currentLevelData.MenuList.Count == 0)
            return;

        int randomIndex = Random.Range(0, _currentLevelData.MenuList.Count);
        int newMenuId = _currentLevelData.MenuList[randomIndex].ID;

        if (_gameState.PlayerAssignments.ContainsKey(playerId))
        {
            var assignment = _gameState.PlayerAssignments[playerId];
            assignment.AssignedMenuId = newMenuId;
            _gameState.PlayerAssignments.Set(playerId, assignment);
        }
        else
        {
            Debug.LogWarning($"[FusionQuestService] Player {playerId} not found in PlayerAssignments.");
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager_Offline
{
    // --- Internal State ---
    private List<Menu> _menuPool = new List<Menu>();
    private int _requiredCount;
    private int _completedCount;
    private Dictionary<int, int> _playerAssignments = new Dictionary<int, int>();

    // --- Public Properties (for UI to read) ---
    public int CompletedCount => _completedCount;
    public int RequiredCount => _requiredCount;
    public IReadOnlyDictionary<int, int> PlayerAssignments => _playerAssignments;
    public bool IsLevelComplete => _completedCount >= _requiredCount;

    // --- C# Events for UI Updates ---
    public event Action OnQuestStateChanged;

    public void SetupLevel(LevelData levelData, List<PlayerOffline> players)
    {
        _menuPool = new List<Menu>(levelData.MenuList);
        _requiredCount = levelData.RequestCount;
        _completedCount = 0;
        _playerAssignments.Clear();

        foreach (var player in players)
        {
            AssignNewQuestToPlayer(player.Id);
        }

        OnQuestStateChanged?.Invoke(); 
    }

    public bool SubmitMenu(int playerId, int menuId)
    {
        if (!_playerAssignments.ContainsKey(playerId) || _playerAssignments[playerId] != menuId)
        {
            return false;
        }

        _completedCount++;
        AssignNewQuestToPlayer(playerId);

        OnQuestStateChanged?.Invoke();
        return true;
    }

    public int GetPlayerAssignment(int playerId)
    {
        return _playerAssignments[playerId];
    }

    public void AssignNewQuestToPlayer(int playerId)
    {
        if (_menuPool.Count == 0)
        {
            _playerAssignments[playerId] = -1; 
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, _menuPool.Count);
        int newMenuId = _menuPool[randomIndex].ID;
        Debug.Log($"newMenuId = {newMenuId}");

        _playerAssignments[playerId] = newMenuId;
    }
}

using UnityEngine;

public class LevelProgression
{
    private readonly GameModeData _gameModeData;
    private int _currentLevelIndex = -1;
    public int CurrentLevelIndex => _currentLevelIndex;
    public LevelData CurrentLevel { get; private set; }

    public LevelProgression(GameModeData gameModeData)
    {
        _gameModeData = gameModeData;
    }
    public bool IsRandomLevel()
    {
        if (_currentLevelIndex >= _gameModeData.LevelList.Count - 1)
        {
            return true;
        }
        return false;
    }
    public LevelData AdvanceToNextLevel()
    {
        _currentLevelIndex++;

        if (_currentLevelIndex >= _gameModeData.LevelList.Count - 1)
        {
            CurrentLevel = _gameModeData.GetRandomLevel();
        }
        else
        {
            CurrentLevel = _gameModeData.LevelList[_currentLevelIndex];
        }
        return CurrentLevel;
    }
}
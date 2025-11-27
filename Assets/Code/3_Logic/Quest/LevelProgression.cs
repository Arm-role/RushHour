using UnityEngine;

public class LevelProgression
{
    private readonly GameModeData _gameModeData;
    private int _currentLevelIndex = -1;
    private int _randRequestCount = 20;

    public int CurrentLevelIndex => _currentLevelIndex;
    public LevelData CurrentLevel { get; private set; }

    public LevelProgression(GameModeData gameModeData)
    {
        _gameModeData = gameModeData;
    }
    public bool IsRandomLevel()
    {
        if (_currentLevelIndex + 1 >= _gameModeData.RandomStartLevel)
        {
            return true;
        }
        return false;
    }
    public LevelData AdvanceToNextLevel()
    {
        _currentLevelIndex++;

        if (IsRandomLevel())
        {
            CurrentLevel = ScriptableObject.Instantiate(_gameModeData.GetRandomLevel());
            CurrentLevel.RequestCount = _randRequestCount += 3;
        }
        else
        {
            CurrentLevel = _gameModeData.LevelList[_currentLevelIndex];
        }
        return CurrentLevel;
    }
}
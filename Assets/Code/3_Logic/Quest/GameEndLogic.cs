using System;

public class GameEndLogic
{
    private float _totalScore;

    public event Action<float> OnScoreChanged;

    public void Start(float totalScore)
    {
        _totalScore = totalScore;
        OnScoreChanged?.Invoke(_totalScore);
    }
}
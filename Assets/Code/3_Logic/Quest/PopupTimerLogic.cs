using GameEvents;
using System;

public class PopupTimerLogic
{
    private float _timeLeft;
    private bool _isRunning;

    public event Action<float> OnTimeChanged;
    public event Action OnTimerFinished;

    public void Start(float time)
    {
        _timeLeft = time;
        _isRunning = true;
        OnTimeChanged?.Invoke(_timeLeft);
    }

    public void Stop()
    {
        _isRunning = false;
    }

    public void Tick(float deltaTime)
    {
        if (!_isRunning) return;

        _timeLeft -= deltaTime;
        if (_timeLeft < 0) _timeLeft = 0;

        OnTimeChanged?.Invoke(_timeLeft);

        if (_timeLeft <= 0)
        {
            _isRunning = false;
            OnTimerFinished?.Invoke();
        }
    }
}

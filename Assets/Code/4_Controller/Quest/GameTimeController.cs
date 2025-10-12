using GameEvents;
using PlayerEvents;
using System;
using UnityEngine;

public class GameTimeController : MonoBehaviour
{
    [SerializeField] private GameTimeView gameTimeView;

    private float _gameSpeed = 1;
    private float _currentMaxTime;

    private GameTimerLogic _logic;

    public event Action OnGameTimeFinished;
    public bool _isRunning { get; set; }

    private void Awake()
    {
        _logic = new GameTimerLogic();
        _logic.OnTimerFinished += GameTimeFinished;
        EventManager.Subscribe<GameFlow>(OnGameState);
        EventManager.Subscribe<TotalScoreEvent>(SetTime);
    }

    private void SetTime(TotalScoreEvent evt)
    {
        _logic.Start(evt.TotalScore, _currentMaxTime);
    }

    private void OnGameState(GameFlow evt)
    {
        _isRunning = (evt.Flow == EGameFlow.Run);
    }

    private void OnDestroy()
    {
        _logic.OnTimerFinished -= GameTimeFinished;
        EventManager.Unsubscribe<GameFlow>(OnGameState);
        EventManager.Unsubscribe<TotalScoreEvent>(SetTime);
    }
    private void Update()
    {
        if (_isRunning)
        {
            _logic.Tick(Time.deltaTime * _gameSpeed);
        }
    }

    public void StartGameTime(float toatalScore, float maxTime, float gameSpeed)
    {
        _gameSpeed = gameSpeed;
        gameTimeView.Setup(_logic, maxTime);
        _currentMaxTime = maxTime;
        _logic.Start(toatalScore, maxTime);
    }
    private void GameTimeFinished()
    {
        OnGameTimeFinished?.Invoke();
    }
}

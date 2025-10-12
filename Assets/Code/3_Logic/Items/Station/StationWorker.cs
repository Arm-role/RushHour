using GameEvents;
using UnityEngine;

public class StationWorker : MonoBehaviour
{
    private IWorkStation _currentWork;
    private Station _station;

    private bool _isActive;
    private void Start()
    {
        _isActive = (GameFlowState.Current == EGameFlow.Run);
        EventManager.Subscribe<GameFlow>(OnGameState);
    }
    private void OnDestroy()
    {
        EventManager.Unsubscribe<GameFlow>(OnGameState);
    }

    private void OnGameState(GameFlow evt)
    {
        _isActive = (evt.Flow == EGameFlow.Run);
    }
    public void BeginWork(IWorkStation workStategy, Station station)
    {
        _station = station;
        _currentWork = workStategy;
        _currentWork.OnStart(_station);

        enabled = true;
    }

    private void Update()
    {
        if (!_isActive) return;

        if (_currentWork == null)
        {
            enabled = false;
            return;
        }

        _currentWork.OnUpdate(_station);

        if (_currentWork.IsCancel(_station))
        {
            _currentWork.OnCancel(_station);
            _currentWork = null;
            enabled = false;
        }
        else if (_currentWork.IsComplete(_station))
        {
            _currentWork.OnComplete(_station);
            _currentWork = null;
            enabled = false;
        }
    }
    public void ReceiveExternalInput()
    {
        _currentWork?.OnRecieveExternalInput(_station);
    }
}
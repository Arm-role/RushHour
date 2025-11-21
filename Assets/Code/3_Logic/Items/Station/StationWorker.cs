using GameEvents;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class StationWorker : MonoBehaviour
{
    private IWorkStation _currentWork;
    private Station _station;

    private bool _isActive;
    private void Start()
    {
        _isActive = (GameFlowState.Current == EGameFlow.GamePlay);
        EventManager.Subscribe<GameFlow>(OnGameState);
    }
    private void OnDestroy()
    {
        EventManager.Unsubscribe<GameFlow>(OnGameState);
    }

    private void OnGameState(GameFlow evt)
    {
        _isActive = (evt.Flow == EGameFlow.GamePlay);
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
            CancelWork();
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
    public void ForceCancel()
    {
        if (_currentWork == null) return;

        Debug.Log($"[StationWorker] ForceCancel work on {_station.name}");
        CancelWork();
    }
    private void CancelWork()
    {
        _currentWork.OnCancel(_station);
        _currentWork = null;
        enabled = false;
    }
}
using System.Collections;
using UnityEngine;

public class StationWorker : MonoBehaviour
{
    private IWorkStation _currentWork;
    private Station _station;

    public void BeginWork(IWorkStation workStategy, Station station)
    {
        _station = station;
        _currentWork = workStategy;
        _currentWork.OnStart(_station);

        enabled = true;
    }

    private void Update()
    {
        if (_currentWork == null)
        {
            enabled = false;
            return;
        }

        _currentWork.OnUpdate(_station);

        if (_currentWork.IsComplete(_station))
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
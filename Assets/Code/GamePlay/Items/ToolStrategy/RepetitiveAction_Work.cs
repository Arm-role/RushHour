public class RepetitiveAction_Work : IWorkStation
{
    private float _requestAction;
    private FoodData _cookedTtem;
    private StationEvents _stationEvent;

    public RepetitiveAction_Work(float requestAction, FoodData result, StationEvents stationEvent)
    {
        _requestAction = requestAction;
        _cookedTtem = result;
        _stationEvent = stationEvent;
    }
    public void OnStart(Station station)
    {
        station.stationData.IsWorking = true;
        station.stationData.CurrentTime = 0;
        _stationEvent.OnWorkStarted.Invoke(station);
    }

    public void OnUpdate(Station station) { }

    public void OnRecieveExternalInput(Station station)
    {
        station.stationData.CurrentTime++;
        _stationEvent.OnWorkPogress.Invoke(station);
    }

    public bool IsComplete(Station station)
    {
        return station.stationData.CurrentTime >= _requestAction;
    }

    public void OnComplete(Station station)
    {
        station.stationData.IsWorking = false;
        //ItemEvents.Instance.OnItemEjected.Invoke((_cookedTtem, station.transform.position));
        _stationEvent.OnWorkCompleted.Invoke(station);
    }
}
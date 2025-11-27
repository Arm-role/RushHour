using GameEvents;
using ItemEvents;

public class RepetitiveAction_Work : IWorkStation
{
    private float _requestAction;
    private FoodData _rawItem;
    private FoodData _cookedItem;

    private ToolWorkData _stationData;

    public RepetitiveAction_Work(float requestAction, FoodData rawFood, FoodData cookFood)
    {
        _requestAction = requestAction;
        _rawItem = rawFood;
        _cookedItem = cookFood;
    }
    public void OnStart(Station station)
    {
        _stationData = station.GetData<ToolWorkData>();

        _stationData.IsWorking = true;
        _stationData.ActionCount = 0;
        _stationData.RequiredActions = _requestAction;
        _stationData.SourceItem = _rawItem;
        _stationData.ResultItem = _cookedItem;

        EventManager.Invoke(new ItemAddToStation(station, _rawItem));
        EventManager.Invoke(new WorkStarted(station));
    }

    public void OnUpdate(Station station) { }

    public void OnRecieveExternalInput(Station station)
    {
        _stationData.ActionCount++;
        EventManager.Invoke(new PlaySFXSound("Cut", station.transform.position));
        EventManager.Invoke(new WorkProgress(station));
    }

    public bool IsComplete(Station station)
    {
        return _stationData.ActionCount >= _requestAction;
    }

    public void OnComplete(Station station)
    {
        _stationData.IsWorking = false;

        EventManager.Invoke(new ItemEjected(_cookedItem.Name, station.transform.position));
        EventManager.Invoke(new ItemRemoveFromStation(station, _rawItem));
        EventManager.Invoke(new WorkCompleted(station));
    }

    public bool IsCancel(Station station)
    {
        return _stationData.IsCancel && _stationData.IsWorking;
    }

    public void OnCancel(Station station)
    {
        _stationData.IsWorking = false;
        _stationData.IsCancel = false;

        EventManager.Invoke(new ItemEjected(_rawItem.Name, station.transform.position));
        EventManager.Invoke(new ItemRemoveFromStation(station, _rawItem));
        EventManager.Invoke(new WorkCanceled(station));
    }
}

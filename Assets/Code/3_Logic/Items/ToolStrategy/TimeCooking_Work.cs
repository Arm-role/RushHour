using GameEvents;
using ItemEvents;
using System.Collections;
using UnityEngine;

public class TimeCooking_Work : IWorkStation
{
    private float _requestTime;
    private FoodData _rawItem;
    private FoodData _cookedItem;

    private ToolWorkData _stationData;

    public TimeCooking_Work(float requestTime, FoodData rawFood, FoodData cookFood)
    {
        _requestTime = requestTime;
        _rawItem = rawFood;
        _cookedItem = cookFood;
    }
    public void OnStart(Station station)
    {
        _stationData = station.GetData<ToolWorkData>();

        _stationData.IsWorking = true;
        _stationData.ActionCount = 0;
        _stationData.RequiredActions = _requestTime;
        _stationData.SourceItem = _rawItem;
        _stationData.ResultItem = _cookedItem;

        EventManager.Invoke(new ItemAddToStation(station, _rawItem));
        EventManager.Invoke(new WorkStarted(station));
    }

    public void OnUpdate(Station station)
    {
        _stationData.ActionCount += Time.deltaTime;
        EventManager.Invoke(new WorkProgress(station));
    }

    public void OnRecieveExternalInput(Station station) { }

    public bool IsComplete(Station station)
    {
        return _stationData.ActionCount >= _requestTime;
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

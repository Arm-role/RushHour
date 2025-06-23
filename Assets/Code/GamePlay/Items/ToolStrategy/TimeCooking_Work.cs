using System.Collections;
using UnityEngine;

public class TimeCooking_Work : IWorkStation
{
    private float _requestTime;
    private FoodData _rawItem;
    private FoodData _cookedItem;

    private StationEvents _stationEvent;
    private ItemEvents _itemEvents;
    public TimeCooking_Work(float requestTime, FoodData rawFood, FoodData cookFood, StationEvents stationEvent, ItemEvents itemEvents)
    {
        _requestTime = requestTime;
        _rawItem = rawFood;
        _cookedItem = cookFood;
        _stationEvent = stationEvent;
        _itemEvents = itemEvents;
    }
    public void OnStart(Station station)
    {
        station.stationData.IsWorking = true;
        station.stationData.CurrentTime = 0;
        station.stationData.MaxTime = _requestTime;
        station.stationData.RawFood = _rawItem;
        station.stationData.CookFood = _cookedItem;

        _stationEvent.OnWorkStarted.Invoke(station);
    }

    public void OnUpdate(Station station)
    {
        station.stationData.CurrentTime += Time.deltaTime;
        _stationEvent.OnWorkPogress.Invoke(station);
    }

    public void OnRecieveExternalInput(Station station) { }

    public bool IsComplete(Station station)
    {
        return station.stationData.CurrentTime >= _requestTime;
    }

    public void OnComplete(Station station)
    {
        station.stationData.IsWorking = false;
        _itemEvents.OnItemEjected.Invoke(_cookedItem, station.transform.position);
        _stationEvent.OnWorkCompleted.Invoke(station);
    }
}

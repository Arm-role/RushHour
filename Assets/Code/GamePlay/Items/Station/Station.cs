using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    public EToolType toolType;
    private IInteractionStrategy interactionStrategie;

    [HideInInspector]
    public StationEvents StationEvent;
    [HideInInspector]
    public ItemEvents ItemEvents;

    [HideInInspector]
    public AssetProvider<Item> CacheItem;

    public StationData stationData = new();
    public StationWorker worker { get; private set; }
    private void Awake()
    {
        worker = gameObject.AddComponent<StationWorker>();
        worker.enabled = false;
        interactionStrategie = GetStrategy(toolType);
    }
    public void Initialze(AssetProvider<Item> cacheItem, StationEvents stationEvent, ItemEvents itemEvents)
    {
        CacheItem = cacheItem;
        StationEvent = stationEvent;
        ItemEvents = itemEvents;
    }
    public IInteractionStrategy GetStrategy(EToolType toolType)
    {
        IInteractionStrategy strategy = null;
        switch (toolType)
        {
            case EToolType.Fried:
                strategy = new StartCooking_Interaction();
                break;
        }
        return strategy;
    }
    public bool Interact(InteractableItem sourceItem)
    {
        if (!interactionStrategie.CanExecute(sourceItem, this)) return false;
        interactionStrategie.Execute(sourceItem, this);
        return true;
    }
}

[Serializable]
public class StationData
{
    public float CurrentTime = 0f;
    public float MaxTime = 0f;
    public bool IsWorking = false;
    public FoodData RawFood = null;
    public FoodData CookFood = null;
}

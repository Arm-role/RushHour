using UnityEngine;

public class ItemInitialzeEvent
{
    private readonly AssetProvider<Item> _cacheItem;
    private readonly ItemEvents _itemEvents;

    public ItemInitialzeEvent(AssetProvider<Item> cacheItem, ItemEvents itemEvents)
    {
        _cacheItem = cacheItem;
        _itemEvents = itemEvents;
    }

    public void InitialzeItem(GameObject instance)
    {
        var stationEvent = new StationEvents();

        if (instance.TryGetComponent<Station>(out var station))
        {
            station.Initialze(_cacheItem, stationEvent, _itemEvents);
        }

        if (instance.TryGetComponent<ToolView>(out var toolView))
        {
            toolView.Initialze(stationEvent);
        }
    }
}
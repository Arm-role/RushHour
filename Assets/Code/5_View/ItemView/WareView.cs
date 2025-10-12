using UnityEngine;

public class WareView : MonoBehaviour
{
    [SerializeField] private string _spriteObName;
    [SerializeField] private Transform _keepper;

    private Station _station;
    private OrderLayerSystem _orderLayerSystem;

    private VisualPresenter _presenter;
    private int _currentSortOrder;

    private void Start()
    {
        _station = GetComponent<Station>();
    }
    public void Initialze(OrderLayerSystem orderLayerSystem, GameObjectSpawner objectSpawner)
    {
        _orderLayerSystem = orderLayerSystem;
        _currentSortOrder = _orderLayerSystem.GetHighSortingOrder() + 1;
        _presenter = new VisualPresenter(_spriteObName, _keepper, objectSpawner);

        EventManager.Subscribe<ItemAddToStation>(OnItemAddToStation);
        EventManager.Subscribe<ItemRemoveFromStation>(OnItemRemoveFromStation);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe<ItemAddToStation>(OnItemAddToStation);
        EventManager.Unsubscribe<ItemRemoveFromStation>(OnItemRemoveFromStation);

        _presenter.RemoveItemAll(spriteItem => _orderLayerSystem.Remove(spriteItem.Renderor));
        _currentSortOrder = 1;
    }
    private async void OnItemAddToStation(ItemAddToStation evt)
    {
        if (evt.Station != _station) return;

        int assignSortOrder = _currentSortOrder;
        _currentSortOrder++;

        var spriteItem = await _presenter.AddItem(evt.Item, assignSortOrder, true);
        if (spriteItem != null)
        {
            spriteItem.transform.localScale = evt.Item.localScale;
            _orderLayerSystem.Add(spriteItem.Renderor);
        }
    }
    private void OnItemRemoveFromStation(ItemRemoveFromStation evt)
    {
        if (evt.Station != _station) return;
        _presenter.RemoveItem(evt.Item, spriteItem => _orderLayerSystem.Remove(spriteItem.Renderor));
        _currentSortOrder--;
    }
}
using GameEvents;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ToolView : MonoBehaviour
{
    [SerializeField] private Slider slider;

    [SerializeField] private string _spriteObName;
    [SerializeField] private Transform _keepper;

    private Station _station;
    private OrderLayerSystem _orderLayerSystem;

    private VisualPresenter _presenter;
    private int _currentSortOrder;

    private bool isWorking = false;

    private void Start()
    {
        _station = GetComponent<Station>();
    }
    public void Initialze(OrderLayerSystem orderLayerSystem, GameObjectSpawner objectSpawner)
    {
        _orderLayerSystem = orderLayerSystem;
        _currentSortOrder = _orderLayerSystem.GetHighSortingOrder() + 1;
        _presenter = new VisualPresenter(_spriteObName, _keepper, objectSpawner);

        EventManager.Subscribe<WorkStarted>(OnStart);
        EventManager.Subscribe<WorkProgress>(OnProgress);
        EventManager.Subscribe<WorkCompleted>(OnComplete);
        EventManager.Subscribe<WorkCanceled>(OnCancel);
        EventManager.Subscribe<ItemAddToStation>(OnItemAddToStation);

        EventManager.Subscribe<GameFlow>(OnGameState);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe<WorkStarted>(OnStart);
        EventManager.Unsubscribe<WorkProgress>(OnProgress);
        EventManager.Unsubscribe<WorkCompleted>(OnComplete);
        EventManager.Unsubscribe<WorkCanceled>(OnCancel);
        EventManager.Unsubscribe<ItemAddToStation>(OnItemAddToStation);

        EventManager.Subscribe<GameFlow>(OnGameState);
    }

    private void OnGameState(GameFlow evt)
    {
        if (!isWorking && evt.Flow == EGameFlow.GamePlay) return;
        slider.gameObject.SetActive(evt.Flow == EGameFlow.GamePlay);
    }
    private async void OnItemAddToStation(ItemAddToStation evt)
    {
        if (evt.Station != _station) return;

        int assignSortOrder = _currentSortOrder;
        _currentSortOrder++;

        var spriteItem = await _presenter.AddItem(evt.Item, assignSortOrder);
        if (spriteItem != null)
        {
            spriteItem.transform.localScale = evt.Item.localScale;
            _orderLayerSystem.Add(spriteItem.Renderor);
        }

    }
    private void OnStart(WorkStarted evt)
    {
        if (evt.Station != _station) return;

        var data = evt.Station.GetData<ToolWorkData>();
        slider.maxValue = data.RequiredActions;
        slider.value = 0;
        slider.gameObject.SetActive(true);

        isWorking = true;
    }
    private void OnProgress(WorkProgress evt)
    {
        if (evt.Station != _station) return;
        var data = evt.Station.GetData<ToolWorkData>();

        slider.value = data.ActionCount;
    }
    private void OnComplete(WorkCompleted evt)
    {
        if (evt.Station != _station) return;

        slider.gameObject.SetActive(false);
        isWorking = false;

        _presenter.RemoveItemAll(spriteItem => _orderLayerSystem.Remove(spriteItem.Renderor));
        _currentSortOrder = 1; ;
    }
    private void OnCancel(WorkCanceled evt)
    {
        if (evt.Station != _station) return;

        slider.gameObject.SetActive(false);
        isWorking = false;

        _presenter.RemoveItemAll(spriteItem => _orderLayerSystem.Remove(spriteItem.Renderor));
        _currentSortOrder = 1;
    }
}

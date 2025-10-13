using GameEvents;
using ItemEvents;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderView : MonoBehaviour
{
    [SerializeField] private Slider timeSlider;

    [SerializeField] private string _spriteObName;
    [SerializeField] private Transform _keeper;
    [SerializeField] private Transform _helper;

    [SerializeField] private SpriteRenderer[] _renderorState;

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _itemCountText;

    private OrderLayerSystem _orderLayerSystem;
    private VisualPresenter _presenter;
    private int _currentSortOrder;

    private Station _station;
    private (int, Item) _currentItem;

    private bool _isGameRunning = false;
    private bool _isWorking = false;

    private void Start()
    {
        _station = GetComponent<Station>();
    }
    public void Initialze(OrderLayerSystem orderlayerSystem, GameObjectSpawner objectSpawner, ItemWorkService itemWorkService)
    {
        _orderLayerSystem = orderlayerSystem;
        _currentSortOrder = _orderLayerSystem.GetHighSortingOrder() + 1;
        _presenter = new VisualPresenter(_spriteObName, _keeper, objectSpawner, _helper, itemWorkService);

        EventManager.Subscribe<WorkStarted>(OnStart);
        EventManager.Subscribe<ItemSpawnRequested>(OnSpawnPlate);
        EventManager.Subscribe<WorkProgress>(OnProgress);
        EventManager.Subscribe<WorkCompleted>(OnComplete);
        EventManager.Subscribe<WorkCanceled>(OnCancel);
        EventManager.Subscribe<IngredienAddToOrder>(OnIngredienAddToOrder);

        EventManager.Subscribe<GameFlow>(OnGameState);

        _isGameRunning = (GameFlowState.Current == EGameFlow.Run);
        UpdateSliderState();
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<WorkStarted>(OnStart);
        EventManager.Unsubscribe<ItemSpawnRequested>(OnSpawnPlate);
        EventManager.Unsubscribe<WorkProgress>(OnProgress);
        EventManager.Unsubscribe<WorkCompleted>(OnComplete);
        EventManager.Unsubscribe<WorkCanceled>(OnCancel);
        EventManager.Unsubscribe<IngredienAddToOrder>(OnIngredienAddToOrder);

        EventManager.Unsubscribe<GameFlow>(OnGameState);
    }

    private void OnGameState(GameFlow evt)
    {
        _isGameRunning = (evt.Flow == EGameFlow.Run);
        UpdateSliderState();
    }

    private void OnStart(WorkStarted evt)
    {
        if (evt.Station != _station) return;

        _isWorking = true;

        SetActiveState(0);

        var data = evt.Station.GetData<OrderLifecycleManager>();

        _scoreText.text = $"{data.CurrentScore}P";

        _currentItem = data.GetFirstItemRequest();

        timeSlider.maxValue = data.RequirementData.TimeLimit;
        UpdateSliderState();
    }
    private async void OnSpawnPlate(ItemSpawnRequested _)
    {
        var spriteItem = await _presenter.AddItem(_currentItem.Item2, _currentSortOrder);
        if (spriteItem != null)
        {
            spriteItem.transform.localScale = Vector3.one;
            _orderLayerSystem.Add(spriteItem.Renderor);
            _itemCountText.text = $"X{_currentItem.Item1}";
        }

        var spriteToolItem = await _presenter.AddHelp(_currentItem.Item2, _currentSortOrder);
        if (spriteToolItem != null)
        {
            spriteToolItem.transform.localScale = Vector3.one;
            _orderLayerSystem.Add(spriteToolItem.Renderor);
        }

        SetActiveState(1);
    }

    private async void OnIngredienAddToOrder(IngredienAddToOrder evt)
    {
        if (evt.Station != _station) return;

        ClearIngredienView();

        if (evt.Ingredient.Item1 == 0 && evt.Ingredient.Item2 == null)
        {
            SetActiveState(2);
            return;
        }

        var spriteItem = await _presenter.AddItem(evt.Ingredient.Item2, _currentSortOrder);
        if (spriteItem != null)
        {
            spriteItem.transform.localScale = Vector3.one;
            _orderLayerSystem.Add(spriteItem.Renderor);
            _itemCountText.text = $"X{evt.Ingredient.Item1}";
        }

        var spriteToolItem = await _presenter.AddHelp(evt.Ingredient.Item2, _currentSortOrder);
        if (spriteToolItem != null)
        {
            spriteToolItem.transform.localScale = Vector3.one;
            _orderLayerSystem.Add(spriteToolItem.Renderor);
        }
    }
    private void OnProgress(WorkProgress evt)
    {
        if (evt.Station != _station) return;

        var data = evt.Station.GetData<OrderLifecycleManager>();
        timeSlider.value = data.CurrentTime;
    }
    private void OnComplete(WorkCompleted evt)
    {
        if (evt.Station != _station) return;

        _isWorking = false;
        UpdateSliderState();
        ClearIngredienView();
    }
    private void OnCancel(WorkCanceled evt)
    {
        if (evt.Station != _station) return;

        _isWorking = false;
        UpdateSliderState();
        ClearIngredienView();
    }

    private void SetActiveState(int state)
    {
        foreach (var renderer in _renderorState)
        {
            if (renderer == _renderorState[state])
            {
                renderer.gameObject.SetActive(true);
            }
            else
            {
                renderer.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateSliderState()
    {
        timeSlider.gameObject.SetActive(_isGameRunning && _isWorking);
    }

    private void ClearIngredienView()
    {
        _presenter.RemoveItemAll(spriteItem => _orderLayerSystem.Remove(spriteItem.Renderor));
        _presenter.RemoveHelpAll(spriteToolItem => _orderLayerSystem.Remove(spriteToolItem.Renderor));
    }
}

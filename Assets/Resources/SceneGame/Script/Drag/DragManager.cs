using System;
using UnityEditor;
using UnityEngine;

public class DragManager : StateMachine<DragManager>
{
    private Rigidbody2D _currentRb;
    private Collider2D _currentColl;
    private ItemHandle _currentHandle;
    private ItemRendering _currentRendering;
    private GameObject _currentItem;
    private EKitchenType _currentType = EKitchenType.Ware;

    public GameObject currentItem => _currentItem;
    public EKitchenType currentType => _currentType;
    public ItemHandle currentHandle => _currentHandle;

    [HideInInspector] public Collider2D target;

    #region Action

    public Action<bool> OnHolder;
    public Action<bool> OnIsTap;
    public Action<bool> OnTouchItem;

    #endregion


    private void Start()
    {
        OnTouchItem += SetIsTrigger;
        OnTouchItem += SetSortOrder;

        SetState(new Idle_DragState());
    }



    #region Process

    private void Update()
    {
        if (GameEvents.Instance.IsGameRun) Execute();
    }
    public void ItemOn(IDropItemTo drop) => drop?.Execute(this);
    #endregion




    #region Process2

    public void SetItem(Collider2D hitColl)
    {
        _currentItem = hitColl.gameObject;
        _currentRb = _currentItem.GetComponent<Rigidbody2D>();
        _currentColl = _currentItem.GetComponent<Collider2D>();
        _currentHandle = _currentItem.GetComponent<ItemHandle>();
        _currentRendering = _currentItem.GetComponent<ItemRendering>();
        _currentType = currentHandle.kitchenType;
    }
    public void ClearItem()
    {
        _currentItem = null;
        _currentRb = null;
        _currentColl = null;
        _currentHandle = null;
        _currentRendering = null;
        _currentType = EKitchenType.Ware;
    }
    public Vector2 GetTouchPos()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(InputHandle.GetTouchPosition());
        return new Vector2(mouseWorldPos.x, mouseWorldPos.y);
    }
    public void MoveItem(Vector2 newPosition) => _currentRb?.MovePosition(newPosition);
    public void SetIsTrigger(bool isTrigger) => _currentColl.isTrigger = isTrigger;
    public void Self_Destruct() => currentHandle.Self_Destruct();
    public void SetSortOrder(bool isTouch)
    {
        if (isTouch)
        {
            _currentRendering.SetSortingOrder();
        }
        else
        {
            _currentRendering.DefaultSortingOrder();
        }
    }
    #endregion




    #region OnAction

    public void HoldItem(bool isHolder) => OnHolder?.Invoke(isHolder);
    public void TapItem(bool isTap) => OnIsTap?.Invoke(isTap);
    public void PushItem(Action<ItemHandle> kitchenTool) => kitchenTool?.Invoke(_currentHandle);
    public void SetTouchItem(bool isTouch)
    {
        if (_currentItem != null)
        {
            OnTouchItem?.Invoke(isTouch);

            if (currentType == EKitchenType.Food)
            {
                OtherEvents.Instance.OnArrowTouch.Invoke(isTouch);
            }
            if (currentType != EKitchenType.Tool)
            {
                GameEvents.Instance.OnTouchItem.Invoke(isTouch);
            }
        }
    }
    #endregion
}

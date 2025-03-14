using UnityEngine;

public class ItemDrag : StateMachine<ItemDrag>
{
    private float zCoord;
    private Rigidbody2D rb;
    private Collider2D collider2d;

    [HideInInspector] public Collider2D target;

    private ItemRendering itemRendering;
    private ItemHandle itemHandle;

    private bool IsReady = false;
    #region Delegates and Events
    public delegate void ImportFoodNotify(string food);
    public event ImportFoodNotify OnFoodNotify;

    public delegate void TouchMove();
    public event TouchMove OnTouchMove;

    public delegate void HolderStatusChanged(bool isHolder);
    public event HolderStatusChanged OnHolder;

    public delegate void TapStatusChanged(bool isTap);
    public event TapStatusChanged OnIsTap;

    public delegate void PutGamObject(GameObject gameOb);
    public event PutGamObject SendGamObject;

    public delegate void TouchItem(bool isItem);
    public event TouchItem OnTouchItem;

    public delegate void DestroyMe(Item item, GameObject ob);
    public DestroyMe OnDestroyMe;

    #endregion
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        itemRendering = GetComponent<ItemRendering>();
        itemHandle = GetComponent<ItemHandle>();
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;

        OnTouchItem += SetIsTrigger;
        OnTouchItem += SetSortOrder;

        if (gameObject.tag == "item")
        {
            OnTouchItem += EvenManager.OnTouchItem;
            OnTouchItem += EvenManager.OnArrowTouchItem;
        }
        if (gameObject.tag == "Ware")
        {
            OnTouchItem += EvenManager.OnTouchItem;
        }

        SetState(new Idle_DragState());
    }
    private void Update()
    {
        if (IsReady)
        {
            Execute();
        }
        else
        {
            IsReady = EvenManager.IsReady;
        }
    }

    public Vector3 GetTouchWorldPos(Vector2 touchPosition)
    {
        Vector3 touchPoint = new Vector3(touchPosition.x, touchPosition.y, zCoord);
        return Camera.main.ScreenToWorldPoint(touchPoint);
    }
    public void SetIsTrigger(bool isTrigger)
    {
        collider2d.isTrigger = isTrigger;
    }
    public void ItemOn(IDropTo dropTo)
    {
        dropTo?.ItemOn(this);
    }
    public void Self_Destruct()
    {
        OnDestroyMe?.Invoke(itemHandle.Item, gameObject);
    }
    public void SetSortOrder(bool isSetter)
    {
        if (isSetter)
        {
            itemRendering.SetSortingOrder();
        }
        else
        {
            itemRendering.DefaultSortingOrder();
        }
    }
    #region Delegate
    public void MoveTo(Vector3 newPosition)
    {
        rb.MovePosition(newPosition);
        OnTouchMove?.Invoke();
    }
    public void SetHolder(bool isHolder)
    {
        OnHolder?.Invoke(isHolder);
    }
    public void SetTap(bool isTap)
    {
        OnIsTap?.Invoke(isTap);
    }
    public void PushItem()
    {
        SendGamObject?.Invoke(gameObject);
    }
    public void SetTouchItem(bool isTouch)
    {
        OnTouchItem?.Invoke(isTouch);
    }
    #endregion
}

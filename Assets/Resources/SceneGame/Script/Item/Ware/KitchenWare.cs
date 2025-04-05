using System.Collections.Generic;
using UnityEngine;

public class KitchenWare : KitchenItem
{
    private Dictionary<Item, Queue<GameObject>> _itemOnWare = new Dictionary<Item, Queue<GameObject>>();
    private Stack<Item> _stackItem = new Stack<Item>();

    private Item foodLalest;

    [SerializeField]
    private float Time_Origin;

    public override float Timer
    {
        get
        {
            return _timer;
        }
        set
        {
            _timer = value;
            slider.value = _timer;
        }
    }

    protected override void Start()
    {
        base.Start();

        ItemEvents.Instance.OnNextFood.Subscribe(SetTime);

        slider.maxValue = Time_Origin;
        Timer = Time_Origin;
    }
    private void OnDestroy()
    {
        ItemEvents.Instance.OnNextFood.UnSubscribe(SetTime);
    }
    protected override void Update()
    {
        if (GameEvents.Instance.IsGameRun)
        {
            Timer -= Time.deltaTime;

            if (0.1 >= Timer)
            {
                ItemEvents.Instance.OnTimeOut.Invoke();
                Timer = Time_Origin;
            }

            if (InputHandle.GetHoldBack() && TouchOverMe(gameObject)) DropItem();
        }
    }
    private void SetTime()
    {
        Timer = Time_Origin;
    }
    public override void PickUpItem(ItemHandle Handle)
    {
        Item item = Handle.Item;
        foodLalest = item;

        GameObject prefab = item.prefab;
        GameObject texture = CreateOnPlate(prefab.transform.GetChild(0).gameObject, true);

        if (!_itemOnWare.ContainsKey(item))
        {
            _itemOnWare[item] = new Queue<GameObject>();
        }

        _itemOnWare[item].Enqueue(texture);
        _stackItem.Push(item);

        Handle.Self_Destruct();

        ItemEvents.Instance.OnSentToCounter.Invoke(foodLalest);
    }

    public override void DropItem()
    {
        Item item = _stackItem?.Pop();
        GameObject obj = _itemOnWare[item]?.Dequeue();

        if (obj != null && item != null)
        {
            Destroy(obj);
            SpawnManager.Instance.OnSpawnItem(item, transform.position);
        }
    }
    public void ClearIntregients()
    {
        foreach (var queue in _itemOnWare.Values)
        {
            while (queue.Count > 0)
            {
                Destroy(queue.Dequeue());
            }
        }
        _itemOnWare.Clear();
        _stackItem.Clear();
    }
    public List<Item> GetFoodOnWare()
    {
        List<Item> list = new List<Item>();

        foreach (var item in _itemOnWare)
        {
            for (int i = 0; i < item.Value.Count; i++)
            {
                list.Add(item.Key);
            }
        }

        return list;
    }
}
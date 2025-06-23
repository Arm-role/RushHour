//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MenuLabelFactory : MonoBehaviour
//{
//    [SerializeField] private GameObject prefab;
//    [SerializeField] private Transform parent;

//    [SerializeField] private Transform OriginTrans;
//    [SerializeField] private Transform OffsetTrans;

//    private Vector2 OffsetPos
//    {
//        get
//        {
//            return new Vector2(OffsetTrans.position.x, OffsetTrans.position.y);
//        }
//    }
//    private Vector2 OriginPos
//    {
//        get
//        {
//            return new Vector2(OriginTrans.position.x, OffsetTrans.position.y);
//        }
//    }

//    private MenuListPool pooling;
//    private PrefabDIFactory<ItemLabelData> factory;
//    private DIContainerBase container = new();
//    private Dictionary<string, ItemLabelData> items;

//    private void Start()
//    {
//        factory = new(prefab, parent, container);
//        pooling = new(factory, initialSize: 10);

//        ItemEvents.Instance.OnItemLabelDisplay.Subscribe(FoodSetup);
//        ItemEvents.Instance.OnItemReturn.Subscribe(FoodServe);
//        ItemEvents.Instance.OnItemUpdate.Subscribe(
//            (pair) => FoodUpdate(pair.itemName, pair.curAmmount, OffsetPos)
//            );
//    }

//    private void FoodSetup(Item item)
//    {
//        if (items.ContainsKey(item.Name)) return;

//        var itemLabel = pooling.Get();
//        itemLabel.IconSprite = item.sprite;
//        itemLabel.Amount  = item.Amount;

//        items[item.Name].Setup(itemLabel);
//    }
//    private void FoodUpdate(string itemName, int curAmmount, Vector2 target)
//    {
//        if (items.TryGetValue(itemName, out var itemLabel))
//        {
//            itemLabel.DataUpdate(curAmmount, target);
//        }
//    }
//    private void FoodServe()
//    {
//        foreach (var itemLabel in items.Values)
//        {
//            if (itemLabel.Amount  < 0) pooling.Return(itemLabel);
//        }
//    }
//}

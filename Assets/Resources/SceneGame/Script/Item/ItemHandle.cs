using System;
using UnityEngine;

public class ItemHandle : MonoBehaviour
{
    public Item Item;
    public EKitchenType kitchenType => Item.kitchenType;
    public Action<Item, GameObject> OnDestroyMe;
    public void Self_Destruct()
    {
        OnDestroyMe?.Invoke(Item, gameObject);
    }
}

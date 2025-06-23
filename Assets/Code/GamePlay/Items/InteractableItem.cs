using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public Item Item;
    public EItemType itemType => Item.itemType;
    public Collider2D LastTarget { get; set; }
    public OrderLayerSystem orderLayerSystem = new();
    public Action<InteractableItem> OnRequestDestruction { get; set; }

    private void Start()
    {
        Transform child = transform.Find("Texture");
        var renderor = child.GetComponent<SpriteRenderer>();
        orderLayerSystem.Add(renderor);
    }
    public void ResetOrder() => orderLayerSystem.ResetOrder();
    public void SetOrderFront() => orderLayerSystem.SetOrderFront();
    public void RequestDestruction() => OnRequestDestruction?.Invoke(this);
}

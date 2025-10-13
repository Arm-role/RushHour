using System;
using GameEvents;
using UnityEngine;

public class InteractableItem : MonoBehaviour, IInteractable
{
    public Item Item;
    public EItemType itemType => Item.itemType;
    public Collider2D LastTarget { get; set; }
    public OrderLayerSystem OrderLayerSystem = new();
    public Transform TextureChild { get; private set; }
    public Action OnDrag { get; set; }
    public Action OnRelease { get; set; }
    public Action OnLateUpdate { get; set; }
    public Action<InteractableItem> OnRequestDestruction { get; set; }

    private void Start()
    {
        Transform textureRoot = transform.Find("Texture");
        if (textureRoot != null)
        {
            foreach (var renderer in textureRoot.GetComponentsInChildren<SpriteRenderer>())
            {
                OrderLayerSystem.Add(renderer);
            }

            foreach (var canvas in textureRoot.GetComponentsInChildren<Canvas>(true))
            {
                OrderLayerSystem.Add(canvas);
            }
        }

        OnDrag += () => OrderLayerSystem.SetAllOrderToFront();
        OnRelease += () => OrderLayerSystem.ResetAllOrder();
    }
    public void Drag() => OnDrag?.Invoke();
    public void Release() => OnRelease?.Invoke();
    private void LateUpdate() => OnLateUpdate?.Invoke();
    public void RequestDestruction()
    {
        EventManager.Invoke(new PlayParticle("Smoke", transform.position));
        OnRequestDestruction?.Invoke(this);
    }
}

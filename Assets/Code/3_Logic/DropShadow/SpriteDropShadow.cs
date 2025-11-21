using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDropShadow : MonoBehaviour
{
    [SerializeField] private Vector2 offset = new Vector2(0, -0.2f);
    [SerializeField] private Sprite sprite;

    private SpriteRenderer shadowRenderer;
    private Transform transShadow;

    private OrderLayerSystem orderLayerSystem;
    private InteractableItem interactableItem;

    private ComponentHolder componentHolder;

    private int sortingLayerID = -1;
   
    private void Start()
    {
        interactableItem = GetComponent<InteractableItem>();
        orderLayerSystem = interactableItem.OrderLayerSystem;

        componentHolder = GetComponent<ComponentHolder>();
        sortingLayerID = componentHolder.GetComponentByName<SpriteRenderer>("Base").sortingLayerID;

        CreateObject(orderLayerSystem, interactableItem);
    }

    private void CreateObject(OrderLayerSystem orderLayerSystem, InteractableItem interactableItem)
    {
        transShadow = new GameObject("DropShadow").transform;

        transShadow.parent = transform;
        transShadow.localRotation = Quaternion.identity;

        shadowRenderer = transShadow.gameObject.AddComponent<SpriteRenderer>();
        shadowRenderer.sortingLayerID = sortingLayerID;
        shadowRenderer.sortingOrder = -1;
        shadowRenderer.color = new Color(0, 0, 0, 0.5f);

        orderLayerSystem.Add(shadowRenderer);

        interactableItem.OnDrag += () =>
        {
            offset = new Vector2(0, -0.4f);
        };
        interactableItem.OnRelease += () =>
        {
            offset = new Vector2(0, -0.2f);
        };

        interactableItem.OnLateUpdate += () =>
        {
            transShadow.position = new Vector2(transform.position.x + offset.x,
            transform.position.y + offset.y);
            shadowRenderer.sprite = sprite;
        };
    }
}

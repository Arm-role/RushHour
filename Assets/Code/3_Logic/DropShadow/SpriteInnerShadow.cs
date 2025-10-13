using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteInnerShadow : MonoBehaviour
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private Transform parent;

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
        transShadow = new GameObject("InnerShadow").transform;
        transShadow.parent = transform;
        transShadow.position = parent.position;

        shadowRenderer = transShadow.gameObject.AddComponent<SpriteRenderer>();
        shadowRenderer.sortingLayerID = sortingLayerID;
        shadowRenderer.sortingOrder = 1;
        shadowRenderer.color = new Color(0, 0, 0, 0.25f);

        orderLayerSystem.Add(shadowRenderer);
        interactableItem.OnLateUpdate += () =>
        {
            transShadow.localRotation = Quaternion.Inverse(transform.localRotation);
            shadowRenderer.sprite = sprite;
        };
    }
}

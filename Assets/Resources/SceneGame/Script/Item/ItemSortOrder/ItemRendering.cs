using System.Collections.Generic;
using UnityEngine;

public class ItemRendering : MonoBehaviour
{
    private List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    private List<int> sortingOrders = new List<int>();

    public int sortingOrder { get; private set; }
    private int sortingOrderOffset = 10;

    private void Start()
    {
        SpriteRenderer renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        sortingOrder = renderer.sortingOrder;

        AddRenderor(renderer);
        AddSortingOrder();
    }


    public void AddRenderor(SpriteRenderer renderer) 
    {
        if (!renderers.Contains(renderer))
        {
            renderers.Add(renderer);
        }
    }
    public void AddSortingOrder() => sortingOrders.Add(sortingOrder);
    public void AddSortingOrder(int sortingOrder) => sortingOrders.Add(sortingOrder);


    public void SetSortingOrder()
    {
        foreach (var renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.sortingOrder = renderer.sortingOrder += sortingOrderOffset;
            }
        }
    }
    public void DefaultSortingOrder()
    {
        ClearRender();
        if (sortingOrders.Count > 0 && sortingOrders.Count == sortingOrders.Count)
        {
            for (int i = 0; i < renderers.Count; i++)
            {
                if (renderers[i] != null)
                {
                    renderers[i].sortingOrder = sortingOrders[i];
                }
            }
        }
    }

    private void ClearRender()
    {
        List<SpriteRenderer> renderer = renderers;
        List<int> sortingOrder = sortingOrders;

        for (int i = 0; i < renderers.Count; i++)
        {
            if (renderer[i] == null)
            {
                renderer.Remove(renderer[i]);
                sortingOrder.Remove(sortingOrder[i]);
            }
        }
        renderers = renderer;
        sortingOrders = sortingOrder;
    }
}

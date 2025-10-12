using Fusion;
using System;
using UnityEngine;

public class SpriteItem : MonoBehaviour
{
    private int _defaultSortingOrder;
    private Vector3 _defaultScale;
    public SpriteRenderer Renderor { get; set; }
    public Action<GameObject> OnRequestDestruction { get; set; }

    private void Start()
    {
        Renderor = GetComponent<SpriteRenderer>();
        _defaultSortingOrder = Renderor.sortingOrder;
        _defaultScale = transform.localScale;
    }
    public void RequestDestruction()
    {
        Renderor.sortingOrder = _defaultSortingOrder;
        transform.localScale = _defaultScale;

        OnRequestDestruction?.Invoke(gameObject);
    }
}
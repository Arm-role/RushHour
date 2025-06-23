using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GridLayoutItemLabel : MonoBehaviour
{
    //[Header("Grid Settings")]
    //public float totalWidth = 10f;        // ความกว้างของ Grid ทั้งหมด

    //private int childCount = 0;

    //public static Action OnComplete;
    //public static Action<ItemLabelData> OnDataUpdate;
    //private void Start()
    //{
    //    OnComplete = Complete;
    //    OnDataUpdate = ChildUpdate;
    //}
    //private void Update()
    //{
    //    if (childCount != transform.childCount)
    //    {
    //        childCount = transform.childCount;
    //        ChildSetUp();
    //    }
    //}

    //private void Complete()
    //{
    //    Debug.Log("Complete");
    //}
    ////[ContextMenu("Reposition Children")]
    //public void SetTargetPosition()
    //{
    //    int count = transform.childCount;
    //    if (count == 0) return;

    //    float spacing = count > 1 ? totalWidth / (count - 1) : 0f;
    //    if (spacing >= 2.6f)
    //    {
    //        spacing = 2.6f;
    //    }
    //    else if (spacing <= 2f)
    //    {
    //        spacing = 2f;
    //    }

    //    float startX = 0;
    //    Vector2 offset = new();
    //    float x = 0;

    //    for (int i = 0; i < count; i++)
    //    {
    //        if (i > 9)
    //        {
    //            offset = new Vector2(x, 0);
    //            x = offset.x;
    //        }
    //        else if (i > 6)
    //        {
    //            float newStart = offset.x;
    //            x = newStart + (i - 6) * 0.5f;
    //        }
    //        else
    //        {
    //            x = startX + i * spacing;
    //            offset = new Vector2(x, 0);
    //        }
    //        var item = transform.GetChild(i).GetComponent<ItemLabelData>();
    //        item.Target = new Vector2(transform.position.x + x, transform.position.y); ;
    //    }
    //}
    //private void ChildSetUp()
    //{
    //    var children = new List<Transform>();
    //    var childData = new Dictionary<Transform, ItemLabelData>();
    //    foreach (Transform child in transform)
    //    {
    //        children.Add(child);
    //        childData[child] = child.GetComponent<ItemLabelData>();
    //    }

    //    SetSiblingIndexAll(children);
    //    SetOrder(children, childData);
    //    SetTargetPosition();
    //}
    //private void ChildUpdate(ItemLabelData itemData)
    //{
    //    var children = new List<Transform>();
    //    var childData = new Dictionary<Transform, ItemLabelData>();
    //    foreach (Transform child in transform)
    //    {
    //        children.Add(child);
    //        childData[child] = child.GetComponent<ItemLabelData>();
    //    }

    //    SetOrder(children, childData);
    //    SetTargetPosition();
    //}
    //private void SetSiblingIndexAll(List<Transform> children)
    //{
    //    for (int i = 0; i < children.Count; i++)
    //    {
    //        children[i].SetSiblingIndex(i);
    //    }
    //}
    //private void SetOrder(List<Transform> children, Dictionary<Transform, ItemLabelData> childData)
    //{
    //    for (int i = 0; i < children.Count; i++)
    //    {
    //        var child = children[i];

    //        if (childData[child].OrderInLayer != children.Count - i)
    //        {
    //            childData[child].OrderInLayer = children.Count - i;
    //        }
    //    }
    //}
}

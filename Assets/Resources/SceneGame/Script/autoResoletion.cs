using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoResoletion : MonoBehaviour
{
    [SerializeField]
    private RectTransform[] rectTransforms;

    private RectTransform RectTransform;
    void Start()
    {
        RectTransform = GetComponent<RectTransform>();
        float Width = RectTransform.rect.width/2;
        foreach (var rectTransform in rectTransforms)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Width);
        }
    }
}

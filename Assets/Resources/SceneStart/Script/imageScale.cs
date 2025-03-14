using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class imageScale : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        float screenHeight = Screen.height;

        while (rectTransform.rect.height < screenHeight)
        {
            rectTransform.localScale += new Vector3(0.1f, 0.1f, 0);
        }
    }
}

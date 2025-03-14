using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTransport : MonoBehaviour
{
    public GameObject texture;
    private Collider2D collider2d;
    private void Start()
    {
        collider2d = GetComponent<Collider2D>();

        EvenManager.ArrowTouchItemChanged += GetDelegate;
    }
    public void GetDelegate(bool isTouch)
    {
        if (collider2d != null && texture != null)
        {
            texture.SetActive(isTouch);
            collider2d.enabled = isTouch;
        }
    }
}

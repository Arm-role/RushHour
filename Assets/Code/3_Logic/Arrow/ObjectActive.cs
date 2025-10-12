using System;
using UnityEngine;

public class ObjectActive : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
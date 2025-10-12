using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggle : MonoBehaviour
{
    public void ActiveObject(GameObject go)
    {
        go.SetActive(true);
    }
    public void UnActiveObject(GameObject go)
    {
        go.SetActive(false);
    }
}

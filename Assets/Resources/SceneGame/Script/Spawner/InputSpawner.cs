using System;
using System.Collections.Generic;
using UnityEngine;

public class InputSpawner : MonoBehaviour
{
    public float ForcePower = 1;
    public List<Transform> SpawnPoint;
    public Action OnDestroyNotify;
    private void Awake()
    {
        SpawnManager.Instance.InputSpawner = this;
        OnDestroyNotify = SpawnManager.Instance.ItemCreator.ClearItem;
    }
    private void OnDestroy()
    {
        OnDestroyNotify?.Invoke();
    }
}

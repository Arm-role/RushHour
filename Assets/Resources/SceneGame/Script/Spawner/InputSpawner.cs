using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSpawner : MonoBehaviour
{
    public float ForcePower = 1;
    public List<Transform> SpawnPoint;
    private void Awake()
    {
        SpawnManager.Instance.RegisterDI<InputSpawner>(this);
    }
}

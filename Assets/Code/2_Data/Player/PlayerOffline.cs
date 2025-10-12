using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffline : MonoBehaviour
{
    public int Id { get; private set; }
    public string Name { get; private set; }

    public PlayerOffline(int id, string name)
    {
        Id = id;
        Name = name;
    }
}

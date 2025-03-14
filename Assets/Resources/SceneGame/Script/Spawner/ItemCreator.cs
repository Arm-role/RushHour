﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreator
{
    private Dictionary<string, Queue<GameObject>> Items = new Dictionary<string, Queue<GameObject>>();

    public GameObject Spawn(Item item, Vector2 position)
    {
        GameObject ob = GetItemFromPool(item);
        SetUpItem(ob, position);
        ob.name = item.name;

        return ob;
    }

    public GameObject Spawn(Item item, float ForcePower, List<Transform> SpawnPoint)
    {
        GameObject ob = GetItemFromPool(item);
        SetUpItem_RandomPoint(ob, ForcePower, SpawnPoint);
        ob.name = item.name;

        return ob;
    }


    public GameObject GetItemFromPool(Item item)
    {
        string foodName = item.Name;
        if (Items.ContainsKey(foodName))
        {
            if (Items[foodName].Count > 0)
            {
                return Items[foodName].Dequeue();
            }
        }
        return Object.Instantiate(item.prefab);
    }


    public void AddToPool(Item item, GameObject ob)
    {
        string foodName = item.Name;

        if (!Items.ContainsKey(foodName))
        {
            Items[foodName] = new Queue<GameObject>();
        }

        ob.SetActive(false);
        Items[foodName].Enqueue(ob);

    }


    private void SetUpItem(GameObject ob, Vector2 position)
    {
        ob.transform.position = position;
        ob.SetActive(true);

        var itemDrag = ob.GetComponent<ItemDrag>();
        itemDrag.OnDestroyMe += AddToPool;
    }

    private void SetUpItem_RandomPoint(GameObject ob, float ForcePower, List<Transform> SpawnPoint)
    {
        int Randomer = Random.Range(0, SpawnPoint.Count);
        Transform spawnPoint = SpawnPoint[Randomer];

        ob.transform.SetParent(spawnPoint);
        ob.transform.position = spawnPoint.position;
        ob.SetActive(true);

        var rigid = ob.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.down * ForcePower, ForceMode2D.Impulse);

        var itemDrag = ob.GetComponent<ItemDrag>();
        itemDrag.OnDestroyMe += AddToPool;
    }
}

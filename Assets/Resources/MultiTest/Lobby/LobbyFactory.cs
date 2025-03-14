using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobbyFactory
{
    private GameObject prefab;
    private Transform parent;

    public LobbyFactory(GameObject prefab, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
    }
    public PlayerLobbyData CreatePlayerLobby(PlayerNetwork playerNetwork)
    {
        PlayerRef player = playerNetwork.playerRef;

        string ObjectName = player.PlayerId.ToString();
        string playerName = playerNetwork.playerName.ToString();

        GameObject playerObject = Object.Instantiate(prefab, parent);
        PlayerLobbyData playerLobbyData;

        playerObject.transform.localScale = Vector3.one;
        playerObject.gameObject.name = ObjectName;

        if (playerObject.transform.TryGetComponent(out playerLobbyData))
        {
            playerLobbyData.GetReady(false);
            playerLobbyData.Name = playerName;
            playerLobbyData.player = player;
        }

        SortChild();

        return playerLobbyData;
    }

    private void SortChild()
    {
        var children = new List<Transform>();

        foreach (Transform child in parent)
        {
            children.Add(child);
        }

        children = children.OrderBy(child => child.name).ToList();

        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }
}

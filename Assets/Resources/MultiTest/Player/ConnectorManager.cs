using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public static class ConnectorManager
{
    private static Dictionary<PlayerNetwork, List<int>> SortIDEachPlayer(Menu menu)
    {
        int[] ids = GetIdFromMenu(menu);
        Dictionary<PlayerNetwork, List<int>> itemIds = new Dictionary<PlayerNetwork, List<int>>();
        var players = PlayerManager.Instance.GetPlayerList();
        int playerCount = players.Count;

        for (int i = 0; i < ids.Length; i++)
        {
            int playerIndex = Random.Range(0, playerCount);
            PlayerNetwork playNet = players[playerIndex];

            if (playNet != null)
            {
                if (!itemIds.ContainsKey(playNet))
                {
                    itemIds[playNet] = new List<int>();
                }

                itemIds[playNet].Add(ids[i]);
            }
        }
        return itemIds;
    }
    private static int[] GetIdFromMenu(Menu menu)
    {
        IEnumerable<ItemAndCount> CombinedList = menu.FoodSpanw.Union(menu.OtherFoodSpawn);
        List<int> Ids = new List<int>();

        foreach (ItemAndCount item in CombinedList)
        {
            for (int i = 0; i < item.count; i++)
            {
                int id = item.item.ID;
                Ids.Add(id);
            }
        }
        return Ids.ToArray();
    }
    public static void TranferToPlayer(Menu menu)
    {
        var itemIds = SortIDEachPlayer(menu);

        foreach (var pair in itemIds)
        {
            int[] id = pair.Value.ToArray();
            byte[] data = IntConverter.PackRLEIDs(id);

            PlayerNetwork player = DIPlayerContain.Instance.LocalPlayerNetwork();
            player.TransportItems(pair.Key, data);
        }
    }
}

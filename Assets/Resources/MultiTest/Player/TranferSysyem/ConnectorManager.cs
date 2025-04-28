using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ConnectorManager
{
    private static ISentItem sentItem = new SentItem();
    private static Dictionary<PlayerNetwork, List<int>> SortIDEachPlayer(Menu menu)
    {
        int[] ids = GetIdFromMenu(menu);
        Dictionary<PlayerNetwork, List<int>> playerIDs = new Dictionary<PlayerNetwork, List<int>>();

        var containPlayer = DIPlayerContain.Instance.GetAllPlayers().Values;
        List<PlayerNetwork> players = new List<PlayerNetwork>(containPlayer);

        int playerCount = players.Count;

        for (int i = 0; i < ids.Length; i++)
        {
            int playerIndex = Random.Range(0, playerCount);

            PlayerNetwork playNet = players[playerIndex];

            if (playNet != null)
            {
                if (!playerIDs.ContainsKey(playNet))
                {
                    playerIDs[playNet] = new List<int>();
                }

                playerIDs[playNet].Add(ids[i]);
            }
        }
        return playerIDs;
    }
    private static int[] GetIdFromMenu(Menu menu)
    {
        IEnumerable<ItemAndCount> CombinedList = menu.FoodSpanw.Union(menu.OtherFoodSpawn);
        List<int> Ids = new List<int>();

        foreach (ItemAndCount item in CombinedList)
        {
            for (int i = 0; i < item.count; i++)
            {
                //Debug.Log(item.item.Name);

                int id = item.item.ID;
                Ids.Add(id);
            }
        }
        return Ids.ToArray();
    }
    public static void TranferToPlayer(Menu menu)
    {
        //Debug.Log(menu.name);

        var playerIDs = SortIDEachPlayer(menu);
        var localPlayer = DIPlayerContain.Instance.LocalPlayer;

        foreach (var pair in playerIDs)
        {
            sentItem.OnSentItems(localPlayer, pair.Key, pair.Value.ToArray());
        }
    }
}

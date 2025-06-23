using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrderByItemPlayers
{
    private ItemEvents _itemEvents;

    public OrderByItemPlayers(ItemEvents itemEvents)
    {
        _itemEvents = itemEvents;
    }

    private Dictionary<PlayerNetwork, List<int>> SortIDEachPlayer(Menu menu)
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
        IEnumerable<Item> CombinedList = menu.FoodSpanw.Union(menu.OtherFoodSpawn);
        List<int> Ids = new List<int>();

        //foreach (Item item in CombinedList)
        //{
        //    for (int i = 0; i < item.Amount; i++)
        //    {
        //        int id = item.ID;
        //        Ids.Add(id);
        //    }
        //}
        return Ids.ToArray();
    }
    public void TranferToPlayer(Menu menu)
    {
        var playerIDs = SortIDEachPlayer(menu);
        var localPlayer = DIPlayerContain.Instance.LocalPlayer;

        foreach (var pair in playerIDs)
        {
            _itemEvents.OnItemsTransported.Invoke(localPlayer, pair.Key, pair.Value.ToArray());
        }
    }









    public Dictionary<PlayerNetwork, List<int>> SortItemToPlayer(int[] itemID)
    {
        Dictionary<PlayerNetwork, List<int>> playerIDs = new Dictionary<PlayerNetwork, List<int>>();

        var containPlayer = DIPlayerContain.Instance.GetAllPlayers().Values;
        List<PlayerNetwork> players = new List<PlayerNetwork>(containPlayer);

        if (itemID.Length < players.Count)
        {
            for (int i = 0; i < players.Count; i++)
            {
                PlayerNetwork playNet = players[i];

                if (playNet != null)
                {
                    if (!playerIDs.ContainsKey(playNet))
                    {
                        playerIDs[playNet] = new List<int>();
                    }
                    int itemIndex = i % players.Count;

                    playerIDs[playNet].Add(itemID[itemIndex]);
                }
            }
        }
        else
        {
            for (int i = 0; i < itemID.Length; i++)
            {
                int playerIndex = i % players.Count;
                PlayerNetwork playNet = players[playerIndex];

                if (playNet != null)
                {
                    if (!playerIDs.ContainsKey(playNet))
                    {
                        playerIDs[playNet] = new List<int>();
                    }
                    playerIDs[playNet].Add(itemID[i]);
                }
            }
        }
        return playerIDs;
    }
    public void RandomItemToPlayer(int[] itemID)
    {
        var playerIDs = SortItemToPlayer(itemID);
        var localPlayer = DIPlayerContain.Instance.LocalPlayer;

        foreach (var pair in playerIDs)
        {
            _itemEvents.OnItemsTransported.Invoke(localPlayer, pair.Key, pair.Value.ToArray());
        }
    }
}

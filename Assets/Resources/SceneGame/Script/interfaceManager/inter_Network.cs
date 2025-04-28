using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerSevice
{
    PlayerNetwork GetPlayer(PlayerRef playerRef);
    IReadOnlyDictionary<PlayerRef, PlayerNetwork> GetAllplayer();
    PlayerNetwork GetNearPlayer(int index);
}

public interface ISentItem
{
    void OnSentItem(PlayerNetwork from, PlayerNetwork to, int ids);
    void OnSentItems(PlayerNetwork from, PlayerNetwork to, int[] ids);
}
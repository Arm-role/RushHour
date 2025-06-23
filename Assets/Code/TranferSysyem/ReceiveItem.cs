using Fusion;
using UnityEngine;

public class ReceiveItem
{
    private ItemEvents _itemEvents;
    public void Initialze(ItemEvents itemEvents)
    {
        _itemEvents = itemEvents;
    }
    public void RPC_SendItem(PlayerRef player, int ID)
    {
        Debug.Log((DIPlayerContain.Instance.LocalPlayer.isLocalPlayer, player));

        if (DIPlayerContain.Instance.LocalPlayer.PlayerRef == player)
        {
            OnReceiveItem(ID);
        }
    }
    public void OnReceiveItem(int id)
    {
        _itemEvents.OnItemIdEjected.Invoke(id);
    }


    public void RPC_SendItems(PlayerRef player, byte[] ID)
    {
        Debug.Log((DIPlayerContain.Instance.LocalPlayer.isLocalPlayer, player));

        if (DIPlayerContain.Instance.LocalPlayer.PlayerRef == player)
        {
            OnReceiveItems(ID);
        }
    }
    public void OnReceiveItems(byte[] ID)
    {
        foreach (int id in IntConverter.UnpackRLEIDs(ID))
        {
            _itemEvents.OnItemIdEjected.Invoke(id);
        }
    }
}

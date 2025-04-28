using Fusion;
using UnityEngine;

public class ReceiveItem
{
    public void RPC_SendItem(PlayerRef player, int ID)
    {
        Debug.Log((DIPlayerContain.Instance.LocalPlayer.isLocalPlayer, player));

        if (DIPlayerContain.Instance.LocalPlayer.PlayerRef == player)
        {
            OnReceiveItem(ID);
        }
    }
    public void OnReceiveItem(int ID)
    {
        Item item = ScriptTable_Contain.instance.GetItem(ID);
        if (item != null)
        {
            ItemEvents.Instance.OnSentItem.Invoke(item);
        }
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
            Item item = ScriptTable_Contain.instance.GetItem(id);

            if (item != null)
            {
                ItemEvents.Instance.OnSentItem.Invoke(item);
            }
        }
    }
}

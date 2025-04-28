using UnityEngine;

public class SentItem : ISentItem
{
    public void OnSentItem(PlayerNetwork from, PlayerNetwork to, int id)
    {
        if (!from.isLocalPlayer) return;
        //Debug.Log("Sent item to " + to.PlayerRef);

        if (from == to) from.OnReceiveItem(id);
        else from.RPC_SendItem(to.PlayerRef, id);
    }

    public void OnSentItems(PlayerNetwork from, PlayerNetwork to, int[] Ids)
    {
        if(!from.isLocalPlayer) return;
        //Debug.Log("Sent menu to " + to.PlayerRef);

        byte[] bytes = IntConverter.PackRLEIDs(Ids);

        if (from == to) from.OnReceiveItems(bytes);
        else from.RPC_SendItems(to.PlayerRef, bytes);
    }
}

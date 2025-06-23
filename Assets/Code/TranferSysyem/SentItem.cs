public class SentItem
{
    private readonly ItemEvents _events;

    public SentItem(ItemEvents events)
    {
        _events = events;
        _events.OnItemTransported.Subscribe(OnSentItem);
        _events.OnItemsTransported.Subscribe(OnSentItems);
    }

    private void OnSentItem(PlayerNetwork from, PlayerNetwork to, int id)
    {
        if (!from.isLocalPlayer) return;

        if (from == to) from.OnReceiveItem(id);
        else from.RPC_SendItem(to.PlayerRef, id);
    }

    private void OnSentItems(PlayerNetwork from, PlayerNetwork to, int[] ids)
    {
        if(!from.isLocalPlayer) return;

        byte[] bytes = IntConverter.PackRLEIDs(ids);

        if (from == to) from.OnReceiveItems(bytes);
        else from.RPC_SendItems(to.PlayerRef, bytes);
    }
}
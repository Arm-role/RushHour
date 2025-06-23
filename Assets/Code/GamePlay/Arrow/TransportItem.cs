using UnityEngine;

public class TransportItem : MonoBehaviour
{
    private ItemLibrary _library;
    private ItemEvents _itemEvent;
    private PlayerManager _playerManager;

    public void Initialze(ItemLibrary library, ItemEvents itemEvent, PlayerManager playerManager)
    {
        _library = library;
        _itemEvent = itemEvent;
        _playerManager = playerManager;
    }
    public void Transport(int targetId, string ItemName)
    {
        int id = _library.FindIdByName(ItemName);

        var owner = _playerManager.GetLocalPlayer();
        var target = _playerManager.GetNearPlayer(targetId);

        _itemEvent.OnItemTransported.Invoke(owner, target, id);
    }
}
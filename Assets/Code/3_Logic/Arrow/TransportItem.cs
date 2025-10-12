using ItemEvents;
using System.Threading.Tasks;
using UnityEngine;

public class TransportItem : MonoBehaviour
{
    private ItemLibrary _library;
    private GameState _gameState;
    public void Initialze(ItemLibrary library)
    {
        _library = library;
        _gameState = FindObjectOfType<GameState>();
    }
    public Task<bool> Transport(int targetId, string itemName)
    {
        if (_gameState == null) return Task.FromResult(false);

        int itemId = _library.FindIdByName(itemName);
        if (itemId == -1) // Assuming -1 means not found
        {
            Debug.LogError($"Item '{itemName}' not found in library.");
            return Task.FromResult(false);
        }

        var owner = PlayerRegistry.Instance.GetLocalPlayer();
        PlayerNetwork target = (targetId == 0) ?
           PlayerService.Instance.LeftNeighbor : PlayerService.Instance.RightNeighbor;

        Debug.Log($"itemId {itemId}");
        Debug.Log($"owner {owner.PlayerRef}");
        Debug.Log($"target {target.PlayerRef}");

        _gameState.RPC_RequestItemTransfer(itemId, owner.PlayerRef, target.PlayerRef);
        return Task.FromResult(true);
    }
}
using Fusion;
using GameEvents;
using System.Linq;
using UnityEngine;
using WebSocketSharp;

public class MasterOrderController 
{
    private readonly IGameState _gameState;
    private readonly IPlayerRepository _playerRepository;
    private readonly IOrderNotifier _orderNotifier;

    public MasterOrderController(IGameState gameState, IPlayerRepository playerRepository, IOrderNotifier orderNotifier)
    {
        _gameState = gameState;
        _playerRepository = playerRepository;
        _orderNotifier = orderNotifier;

        _orderNotifier.OnOrderFulfilled += HandleOrderFulfilled;
    }

    public void Shutdown()
    {
        _orderNotifier.OnOrderFulfilled -= HandleOrderFulfilled;
    }

    private void HandleOrderFulfilled(object sender, OrderFulfilledEventArgs e)
    {
        IPlayer fulfiller = _playerRepository.GetPlayer(e.FulfillerId);
        if (fulfiller != null)
        {
            fulfiller.Score += e.ScoreValue;

            _gameState.CompletedOrderCount++;

            _gameState.TotalScore = _playerRepository.GetAllPlayers().Sum(p => p.Score);
        }
    }
}

using Fusion;
using GameEvents;
using UnityEngine;

public class GameFlowSettup
{
    private EGameFlow _currentFlow;
    public GameFlowSettup()
    {
        EventManager.Subscribe<GameFlow>(OnGameState);
    }
    public void OnGameState(GameFlow evt)
    {
        _currentFlow = evt.Flow;
    }
  
    public bool TryFlow(EGameFlow flow)
    {
        return _currentFlow == flow;
    }
}
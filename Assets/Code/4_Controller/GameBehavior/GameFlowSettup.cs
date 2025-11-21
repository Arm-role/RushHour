using Fusion;
using GameEvents;

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

        switch (evt.Flow)
        {
            case EGameFlow.GameStart: OnStart(); break;
            case EGameFlow.GamePlay:   OnRun();   break;
            case EGameFlow.GameEnd:   OnEnd();   break;
        }
    }
    public void OnStart()
    {
        //PlayerEvents.Instance.OnSetScore?.Invoke(0);
    }
    public void OnRun() { }

    public void OnEnd() { }

    public bool TryFlow(EGameFlow flow)
    {
        return _currentFlow == flow;
    }
}
public class GameState
{
    public void OnGameState(EGameState state)
    {
        switch (state)
        {
            case EGameState.Start: OnStart(); break;
            case EGameState.Run:   OnRun();   break;
            case EGameState.End:   OnEnd();   break;
        }
    }
    public void OnStart()
    {
        GameEvents.Instance.OnSetScore?.Invoke(0);
    }
    public void OnRun() { }

    public void OnEnd() { }
}

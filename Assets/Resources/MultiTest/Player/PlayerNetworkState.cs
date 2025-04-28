public class PlayerLobbyState : IPlayerNetworkState
{
    public void Enter(PlayerNetwork player) { }

    public void Execute(PlayerNetwork player)
    {
        if (GameEvents.Instance.OnGameScene.GetParamiter() == EGameScene.Game)
        {
            player.SetState(new PlayerGameState());
        }
    }

    public void Exit(PlayerNetwork player) { }
}
public class PlayerGameState : IPlayerNetworkState
{
    public void Enter(PlayerNetwork player)
    {
        GameEvents.Instance.OnSentScore.Subscribe(player.AddScore);
        GameEvents.Instance.OnSetScore.Subscribe(player.SetScore);
        PlayerEvents.Instance.OnReady.Subscribe(player.SetReady);
    }

    public void Execute(PlayerNetwork player)
    {
        if (GameEvents.Instance.OnGameScene.GetParamiter() == EGameScene.Lobby)
        {
            player.SetState(new PlayerLobbyState());
        }
    }
    public void Exit(PlayerNetwork player)
    {
        GameEvents.Instance.OnSentScore.UnSubscribe(player.AddScore);
        GameEvents.Instance.OnSetScore.UnSubscribe(player.SetScore);
        PlayerEvents.Instance.OnReady.UnSubscribe(player.SetReady);
    }
}
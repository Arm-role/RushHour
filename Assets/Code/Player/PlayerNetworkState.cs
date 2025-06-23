
public abstract class PlayerSceneBase : IPlayerScene
{
    public virtual void Enter(PlayerNetwork istate) { }
    public virtual void Execute(PlayerNetwork istate) { }
    public virtual void Exit(PlayerNetwork istate) { }

    public void WhenChangeSceneTo<T>(PlayerNetwork player, EGameScene scene) where T : PlayerSceneBase, new()
    {
        //if (GameEvents.Instance.OnGameScene.GetParamiter() == scene)
        //{
        //    player.SetState(new T());
        //}
    }
}

public class PlayerLobbyScene : PlayerSceneBase
{
    public override void Execute(PlayerNetwork player)
    {
        WhenChangeSceneTo<PlayerGameScene>(player, EGameScene.Game);
    }
}
public class PlayerGameScene : PlayerSceneBase
{
    public override void Enter(PlayerNetwork player)
    {
        //PlayerEvents.Instance.OnSentScore.Subscribe(player.AddScore);
        //PlayerEvents.Instance.OnSetScore.Subscribe(player.SetScore);
        //PlayerEvents.Instance.OnReady.Subscribe(player.SetReady);
    }

    public override void Execute(PlayerNetwork player)
    {
        WhenChangeSceneTo<PlayerLobbyScene>(player, EGameScene.Lobby);
    }
    public override void Exit(PlayerNetwork player)
    {
        //PlayerEvents.Instance.OnSentScore.UnSubscribe(player.AddScore);
        //PlayerEvents.Instance.OnSetScore.UnSubscribe(player.SetScore);
        //PlayerEvents.Instance.OnReady.UnSubscribe(player.SetReady);
    }
}
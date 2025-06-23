public interface IPlayerScene : IState<PlayerNetwork>
{
    void WhenChangeSceneTo<T>(PlayerNetwork player, EGameScene scene) where T : PlayerSceneBase, new();
}
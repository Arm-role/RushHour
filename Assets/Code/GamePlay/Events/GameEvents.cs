public class GameEvents
{
    public GameEvents()
    {
        GameState state = new GameState();
        GameScene scene = new GameScene();

        OnGameState.Subscribe(state.OnGameState);
        OnGameScene.Subscribe(scene.OnSceneGame);
    }

    public EventBaseParam<EGameScene> OnGameScene { get; set; } = new GameSceneEvent();
    public EventBaseParam<EGameState> OnGameState { get; set; } = new GameStateEvent();
    public bool IsGameRun => OnGameState?.GetParamiter() == EGameState.Run;

    public EventBase<bool> OnTouchItem { get; set; } = new TouchItemEvent();
    public EventBase<float> TimeSpeed { get; set; } = new TimeSpeedEvent();
}


public sealed class GameSceneEvent : EventBaseParam<EGameScene> { }
public sealed class GameStateEvent : EventBaseParam<EGameState> { }
public sealed class TouchItemEvent : EventBase<bool> { }
public sealed class TimeSpeedEvent : EventBaseParam<float> { }

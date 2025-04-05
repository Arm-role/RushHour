public class GameEvents : SingletonLazy<GameEvents>
{
    public EventBase<float> OnSentScore { get; private set; } = new SentScoreEvent();
    public EventBase<float> OnSetScore { get; set; } = new SetScoreEvent();

    public EventBaseParam<float> OnSentTotalScore { get; set; } = new TotalScoreEvent();
    public float TotalScore => OnSentTotalScore.GetParamiter();

    public EventBase<EGameScene> OnGameScene { get; set; } = new GameSceneEvent();
    public EventBaseParam<EGameState> OnGameState { get; set; } = new GameStateEvent();
    public bool IsGameRun => OnGameState?.GetParamiter() == EGameState.Run;

    public EventBase<bool> OnTouchItem { get; set; } = new TouchItemEvent();

    protected override void OnAwake()
    {
        GameState state = new GameState();
        GameScene scene = new GameScene();

        OnGameState.Subscribe(state.OnGameState);
        OnGameScene.Subscribe(scene.OnSceneGame);
    }
}
public class SentScoreEvent : EventBase<float> { }
public class SetScoreEvent : EventBase<float> { }
public class TotalScoreEvent : EventBaseParam<float> { }

public class GameSceneEvent : EventBase<EGameScene> { }
public class GameStateEvent : EventBaseParam<EGameState> { }
public class TouchItemEvent : EventBase<bool> { }
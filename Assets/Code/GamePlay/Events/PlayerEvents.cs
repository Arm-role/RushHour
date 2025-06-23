
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents 
{
    public static object Instance { get; internal set; }
    public EventBase<bool> OnReady { get; private set; } = new PlayerReady();
    public EventBase<(string, float)> OnSentPlayerScore { get; private set; } = new SentPlayerScore();

    public EventBaseParam<Dictionary<string, float>> OnKeepPlayerScore { get; private set; } = new KeepPlayerScore();


    public EventBase<float> OnSentScore { get; private set; } = new SentScoreEvent();
    public EventBase<float> OnSetScore { get; private set; } = new SetScoreEvent();
    public EventBaseParam<float> OnSentTotalScore { get; private set; } = new TotalScoreEvent();
    public float TotalScore => OnSentTotalScore.GetParamiter();
}



public sealed class PlayerReady : EventBase<bool> { }
public sealed class SentPlayerNetwork : EventBase<PlayerNetwork> { }
public sealed class SentPlayerScore : EventBase<(string, float)> { }
public sealed class KeepPlayerScore : EventBaseParam<Dictionary<string, float>>
{
    public KeepPlayerScore() => _param = new Dictionary<string, float>();
}
public sealed class SentScoreEvent : EventBase<float> { }
public sealed class SetScoreEvent : EventBase<float> { }
public sealed class TotalScoreEvent : EventBaseParam<float> { }



using System.Collections.Generic;

public class PlayerEvents : SingletonLazy<PlayerEvents>
{
    public EventBase<bool> OnReady {  get; set; } = new PlayerReady();
    public EventBase<(string, float)> OnSentPlayerScore { get; set; } = new SentPlayerScore();

    public EventBaseParam<Dictionary<string, float>> OnKeepPlayerScore { get; set; } = new KeepPlayerScore();

    public EventBase<float> TimeSpeed { get; set; } = new TimeSpeedEvent();
}
public class PlayerReady : EventBase<bool> { }
public  class SentPlayerNetwork : EventBase<PlayerNetwork> { }
public class SentPlayerScore : EventBase<(string, float)> { }
public class KeepPlayerScore : EventBaseParam<Dictionary<string, float>>
{
    public KeepPlayerScore() => _param = new Dictionary<string, float>();
}

public class TimeSpeedEvent : EventBaseParam<float> { }



public class PlayerEvents : SingletonLazy<PlayerEvents>
{
    public EventBase<bool> OnReady {  get; set; } = new PlayerReady();
    public EventBase<PlayerNetwork> OnSentPlayerNetwork { get; set; } = new SentPlayerNetwork();
   

    public EventBase<float> TimeSpeed { get; set; } = new TimeSpeedEvent();
}
public class PlayerReady : EventBase<bool> { }
public  class SentPlayerNetwork : EventBase<PlayerNetwork> { }
public class TimeSpeedEvent : EventBaseParam<float> { }


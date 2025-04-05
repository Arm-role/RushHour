public class OtherEvents : SingletonLazy<OtherEvents>
{
    public EventBase<bool> OnArrowTouch { get; set; } = new ArrowTouchItemEvent();
}

public class ArrowTouchItemEvent : EventBase<bool> { }

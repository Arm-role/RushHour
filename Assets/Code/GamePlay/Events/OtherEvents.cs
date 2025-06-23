public class OtherEvents 
{
    public EventBase<bool> OnArrowTouch { get; set; } = new ArrowTouchItemEvent();
}

public sealed class ArrowTouchItemEvent : EventBase<bool> { }

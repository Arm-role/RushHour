
public class MockOrderNotifier : IOrderNotifier
{
    public event System.EventHandler<OrderFulfilledEventArgs> OnOrderFulfilled;

    // Helper method for tests to easily trigger the event
    public void TriggerOrderFulfilled(int score, int fulfillerId)
    {
        OnOrderFulfilled?.Invoke(this, new OrderFulfilledEventArgs(score, fulfillerId));
    }
}
public interface IOrderNotifier
{
    event System.EventHandler<OrderFulfilledEventArgs> OnOrderFulfilled;
}
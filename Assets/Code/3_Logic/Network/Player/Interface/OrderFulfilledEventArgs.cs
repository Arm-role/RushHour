
// --- Notification Interface ---
public class OrderFulfilledEventArgs : System.EventArgs
{
    public int ScoreValue { get; }
    public int FulfillerId { get; }
    public OrderFulfilledEventArgs(int score, int fulfillerId)
    {
        ScoreValue = score;
        FulfillerId = fulfillerId;
    }
}

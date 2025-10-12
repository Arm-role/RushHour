using System;

public class ItemDestroyService
{
    public ItemDestroyService(GameSessionManager gameSessionManager = null)
    {
        if (gameSessionManager == null) return;
 
        gameSessionManager.OnShutdown += RequestDestructionAll;
    }

    public event Action OnRequestDestruction;
    public void RequestDestructionAll() => OnRequestDestruction?.Invoke();
}
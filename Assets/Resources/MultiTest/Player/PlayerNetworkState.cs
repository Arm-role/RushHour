using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLobbyState : IPlayerNetworkState
{
    public void Enter(PlayerNetwork player) { }

    public void Execute(PlayerNetwork player)
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            player.SetState(new PlayerGameState());
        }
    }

    public void Exit(PlayerNetwork player) { }
}
public class PlayerGameState : IPlayerNetworkState
{
    public void Enter(PlayerNetwork player)
    {
        Debug.Log("PlayerGameState");
        if (player.HasStateAuthority == true)
        {
            GameEvents.Instance.OnSentScore.Subscribe(player.GetScore);
            GameEvents.Instance.OnSetScore.Subscribe(player.OnSetScore);
            PlayerEvents.Instance.OnReady.Subscribe(player.OnSetReady);
        }
    }

    public void Execute(PlayerNetwork player) { }
    public void Exit(PlayerNetwork player) { }
}
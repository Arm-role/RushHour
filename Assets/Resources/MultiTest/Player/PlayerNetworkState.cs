using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IPlayerNetworkState
{
    void Enter(PlayerNetwork player);
    void Execute(PlayerNetwork player);
    void Exit(PlayerNetwork player);
}

public class PlayerLobbyState : IPlayerNetworkState
{
    public void Enter(PlayerNetwork player)
    {
    }

    public void Execute(PlayerNetwork player)
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            player.ChangeState(new PlayerGameState());
        }
    }

    public void Exit(PlayerNetwork player)
    {
    }
}
public class PlayerGameState : IPlayerNetworkState
{
    public void Enter(PlayerNetwork player)
    {
        Debug.Log("PlayerGameState");
        if (player.HasStateAuthority == true)
        {
            EvenManager.SentScore += player.GetScore;
        }
    }

    public void Execute(PlayerNetwork player)
    {
    }

    public void Exit(PlayerNetwork player)
    {
    }
}
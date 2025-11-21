using GameEvents;
using PlayerEvents;
using System;
using UnityEngine;

public class GameTimeController : MonoBehaviour
{
    [SerializeField] private GameTimeView gameTimeView;

    private void Start()
    {
        GameState.OnNetworkStateChanged += UpdateView;
    }

    private void OnDestroy()
    {
        GameState.OnNetworkStateChanged -= UpdateView;
    }

    private void UpdateView()
    {
        var gameState = FindObjectOfType<GameState>();
        if (gameState != null)
        {
            //Debug.Log(("TotalScore", gameState.TotalScore, "GameTimer", gameState.GameTimer, "MaxGameTime", gameState.MaxGameTime));
            gameTimeView.UpdateUI(gameState.TotalScore, gameState.GameTimer, gameState.MaxGameTime);
        }
    }
}

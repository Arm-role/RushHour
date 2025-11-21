using GameEvents;
using UnityEngine;

public class GameEndController : MonoBehaviour
{
    [SerializeField] private GameEndView endView;

    private GameState _gameState;
    private void Start()
    {
        _gameState = FindAnyObjectByType<GameState>();
        EventManager.Subscribe<GameFlow>(HandlePhaseChange);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe<GameFlow>(HandlePhaseChange);
    }

    private void HandlePhaseChange(GameFlow evt)
    {
        if (evt.Flow == EGameFlow.GameEnd)
        {
            StartEndSequence(_gameState.TotalScore);
        }
        else
        {
            endView.Hide();
        }
    }

    private void StartEndSequence(float totalScore)
    {
        endView.Show();
        endView.Setup(totalScore);
    }
}

using System;
using UnityEngine;

public class GameEndController : MonoBehaviour
{
    [SerializeField] private GameEndView endView;
    private GameEndLogic _logic;

    private void Awake()
    {
        _logic = new GameEndLogic();
    }

    public void StartEndSequence(float totalScore)
    {
        endView.Show();
        endView.Setup(_logic);
        _logic.Start(totalScore);
    }
}
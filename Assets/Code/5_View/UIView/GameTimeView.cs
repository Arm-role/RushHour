using GameEvents;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Slider timerSlider;

    private void OnEnable()
    {
        EventManager.Subscribe<GameFlow>(HandlePhaseChange);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<GameFlow>(HandlePhaseChange);
    }
    private void HandlePhaseChange(GameFlow ent)
    {
        timerSlider.gameObject.SetActive(ent.Flow == EGameFlow.GamePlay);
        scoreText.gameObject.SetActive(ent.Flow == EGameFlow.GamePlay);
    }

    public void UpdateUI(float totalScore, float timeLeft, float maxTime)
    {
        scoreText.text = $"{totalScore}P";
        float normalized = (maxTime > 0) ? timeLeft / maxTime : 0;
        timerSlider.value = normalized;
    }
}
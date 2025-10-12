using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Slider timerSlider;

    private float _maxTime;

    public void Setup(GameTimerLogic logic, float maxTime)
    {
        _maxTime = maxTime;
        timerSlider.minValue = 0f;
        timerSlider.maxValue = 1f;
        timerSlider.value = 1f;

        logic.OnTimeChanged += UpdateUI;
    }

    private void UpdateUI(float totalScore, float timeLeft)
    {
        scoreText.text = $"{totalScore}P";
        float normalized = timeLeft / _maxTime;
        timerSlider.value = normalized;
    }
}
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupView : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Image popupImage;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider timerSlider;

    private float _maxTime;

    public void Bind(PopupTimerLogic logic, float maxTime)
    {
        _maxTime = maxTime;
        timerSlider.minValue = 0f;
        timerSlider.maxValue = 1f;
        timerSlider.value = 1f;

        logic.OnTimeChanged += UpdateUI;
    }

    private void UpdateUI(float timeLeft)
    {
        float normalized = timeLeft / _maxTime;
        timerSlider.value = normalized;
    }

    public void Hide()
    {
        levelText.gameObject.SetActive(false);
        popupPanel.SetActive(false);
    }

    public void Show(Sprite sprite)
    {
        popupImage.sprite = sprite;
        popupPanel.SetActive(true);
    }
    public void ShowRan(int levelID, Sprite sprite)
    {
        levelText.text = $"LEVEL {levelID}";
        popupImage.sprite = sprite;

        levelText.gameObject.SetActive(true);
        popupPanel.SetActive(true);
    }
}

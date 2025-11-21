using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupView : MonoBehaviour
{
    [Header("UI Elements")]
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

        // ล้าง event ก่อน เพื่อกันซ้ำ
        logic.OnTimeChanged -= UpdateUI;
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
        levelText.gameObject.SetActive(false);
        popupPanel.SetActive(true);
    }

    public void ShowRan(int levelID, Sprite sprite)
    {
        popupImage.sprite = sprite;
        levelText.text = $"LEVEL {levelID}";
        levelText.gameObject.SetActive(true);
        popupPanel.SetActive(true);
    }
}

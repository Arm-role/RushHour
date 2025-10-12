using GameEvents;
using System;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] private PopupView popupView;

    private PopupTimerLogic _logic;

    private PopupData[] _popupSequence; 
    private int _currentPopupIndex = -1;

    public event Action OnPopupFinished;
    private void Awake()
    {
        _logic = new PopupTimerLogic();
    }

    private void Update()
    {
        _logic.Tick(Time.deltaTime);
    }

    public void StartPopupSequence(PopupData[] popupSequence)
    {
        _popupSequence = popupSequence;
        _currentPopupIndex = -1;

        _logic.OnTimerFinished -= HandlePopupFinished;

        _logic.OnTimerFinished += HandlePopupFinished;

        ShowNextPopup();
    }

    private void HandlePopupFinished()
    {
        ShowNextPopup();
    }

    private void ShowNextPopup()
    {
        popupView.Hide();

        _currentPopupIndex++;

        if (_popupSequence == null || _currentPopupIndex >= _popupSequence.Length)
        {
            Debug.Log("Popup sequence finished.");

            _logic.OnTimerFinished -= HandlePopupFinished;
            OnPopupFinished?.Invoke();
            return;
        }

        var popupData = _popupSequence[_currentPopupIndex];

        Debug.Log($"Showing popup index {_currentPopupIndex} for {popupData.timer} seconds.");

        popupView.Bind(_logic, popupData.timer);
        popupView.Show(popupData.StartPopupSprite);
        _logic.Start(popupData.timer);
    }

    private void OnDestroy()
    {
        if (_logic != null)
        {
            _logic.OnTimerFinished -= HandlePopupFinished;
        }
    }
}

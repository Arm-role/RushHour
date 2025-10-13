using GameEvents;
using System;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] private PopupView popupView;
    [SerializeField] private PopupData _randomPopup;

    private PopupTimerLogic _logic;

    private PopupData[] _popupSequence;
    private struct SLevelData
    {
        public bool IsRandom;
        public int LevelAmount;

        public SLevelData(bool isRandom, int levelAmount)
        {
            IsRandom = isRandom;
            LevelAmount = levelAmount;
        }
    }

    private SLevelData levelData;

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

    public void StartPopupSequence(bool isRandomLevel, int levelAmount, LevelData level)
    {
        levelData = new SLevelData(isRandomLevel, levelAmount);

        PopupData[] popupSequence = level.StartPopupSprites;
        _popupSequence = popupSequence;
        _currentPopupIndex = -1;

        _logic.OnTimerFinished -= HandlePopupFinished;

        _logic.OnTimerFinished += HandlePopupFinished;

        ShowNextPopup();
    }

    private void HandlePopupFinished() => ShowNextPopup();

    private void ShowNextPopup()
    {
        popupView.Hide();
        _currentPopupIndex++;

        if (levelData.IsRandom)
        {
            if (_currentPopupIndex > 0)
            {
                Debug.Log("Random popup finished after one show.");
                _logic.OnTimerFinished -= HandlePopupFinished;
                OnPopupFinished?.Invoke();
                return;
            }

            Debug.Log($"Showing random popup for {_randomPopup.timer} seconds.");

            popupView.Bind(_logic, _randomPopup.timer);
            popupView.ShowRan(levelData.LevelAmount, _randomPopup.StartPopupSprite);
            _logic.Start(_randomPopup.timer);
            return;
        }

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

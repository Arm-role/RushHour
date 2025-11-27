using GameEvents;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] private PopupView popupView;
    [SerializeField] private PopupData _randomPopupData;
    [SerializeField] private GameModeData _gameModeData;

    private PopupTimerLogic _logic;
    private GameState _gameState;
    private PopupData[] _popupSequence;
    private int _currentPopupIndex = -1;

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

    private SLevelData _levelData;

    private void Awake()
    {
        _logic = new PopupTimerLogic();
        _gameState = FindAnyObjectByType<GameState>();
    }

    private void Start() => EventManager.Subscribe<GameFlow>(HandleGameFlow);
    private void OnDestroy() => EventManager.Unsubscribe<GameFlow>(HandleGameFlow);

    private void Update() => _logic.Tick(Time.deltaTime);

    private void HandleGameFlow(GameFlow evt)
    {
        if (evt.Flow == EGameFlow.LevelStartPopup)
        {
            Debug.Log("[PopupController] LevelStartPopup triggered");

            if (_gameState == null)
                _gameState = FindAnyObjectByType<GameState>();

            if (_gameState == null)
            {
                Debug.LogWarning("[PopupController] GameState not found");
                return;
            }

            LevelData levelData = _gameModeData.LevelList.Find(i => i.LevelId == _gameState.GlobalLevelIndex);
            StartPopupSequence(_gameState.IsRandomLevel, _gameState.CurrentRunLevelIndex, levelData);
        }
    }

    private void StartPopupSequence(bool isRandomLevel, int levelAmount, LevelData level)
    {
        _levelData = new SLevelData(isRandomLevel, levelAmount);
        _popupSequence = level?.StartPopupSprites;
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

        if (_levelData.IsRandom)
        {
            if (_currentPopupIndex > 0)
            {
                Debug.Log("[PopupController] Random popup finished");
                popupView.Hide();
                _logic.OnTimerFinished -= HandlePopupFinished;
                return;
            }

            popupView.Bind(_logic, _randomPopupData.timer);
            popupView.ShowRan(_levelData.LevelAmount, _randomPopupData.StartPopupSprite);
            _logic.Start(_randomPopupData.timer);
            return;
        }

        if (_popupSequence == null || _currentPopupIndex >= _popupSequence.Length)
        {
            Debug.Log("[PopupController] Popup sequence finished");
            popupView.Hide();
            _logic.OnTimerFinished -= HandlePopupFinished;
            return;
        }

        var popupData = _popupSequence[_currentPopupIndex];
        popupView.Bind(_logic, popupData.timer);
        popupView.Show(popupData.StartPopupSprite);
        _logic.Start(popupData.timer);
    }
}

using GameEvents;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private SoundService _service;
    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _service = GetComponent<SoundService>();

        EventManager.Subscribe<PlayMusicSound>(PlayMusic);
        EventManager.Subscribe<PlaySFXSound>(PlaySFX);
        EventManager.Subscribe<GameFlow>(OnGameState);
    }

    public void OnGameState(GameFlow evt)
    {
        switch (evt.Flow)
        {
            case EGameFlow.GameStart: _service.PlayMusic("StartMusic"); break;
            case EGameFlow.LevelStartPopup: _service.StopMusic(); break;
            case EGameFlow.GamePlay: _service.PlayMusic("GameMusic"); break;
        }
    }

    private void PlayMusic(PlayMusicSound evt)
    {
        _service.PlayMusic(evt.SoundName);
    }

    private void PlaySFX(PlaySFXSound evt)
    {
        _service.PlaySFX(evt.SoundName, evt.Position, evt.Timer);
    }
}

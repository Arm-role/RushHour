using TMPro;
using UnityEngine;

public class Countdown : StateMachine<Countdown>
{
    public GameObject StarUI;
    public TextMeshProUGUI m_TextMeshPro;

    public float timeScala;

    public AudioSource audioSource;

    void Start()
    {
        audioSource = SoundManager.instance.GetComponent<AudioSource>();
        audioSource.clip = SoundManager.instance.soundClips["countdown"];
        audioSource.volume = 0.8f;
        StarUI.SetActive(false);

        GameEvents.Instance.OnGameState.Invoke(EGameState.Start);
        SetState(new StartCoundown());
    }
    void Update() => Execute();
}



public class StartCoundown : ICountdown
{
    public void Enter(Countdown countdown)
    {
        countdown.StarUI.SetActive(true);
        countdown.audioSource.Play();
    }

    public void Execute(Countdown countdown)
    {
        float audioTime = countdown.audioSource.time;

        if (audioTime >= 0 && audioTime < 1)
        {
            countdown.m_TextMeshPro.text = "Ready...";
        }
        else if (audioTime >= 1 && audioTime < 2)
        {
            countdown.m_TextMeshPro.text = "Set...";
        }
        else if (audioTime >= 2 && audioTime < 3)
        {
            countdown.m_TextMeshPro.text = "Go!";
        }
        else if (audioTime >= 3)
        {
            countdown.StarUI.SetActive(false);
            countdown.m_TextMeshPro.text = "Start!";
            SoundManager.instance.PlayLoopClip("gameScene");

            GameEvents.Instance.OnGameState.Invoke(EGameState.Run);
            countdown.SetState(null);
        }
    }

    public void Exit(Countdown istate) { }
}
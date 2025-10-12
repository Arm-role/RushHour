using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectionLostController : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The name of the scene to load. Must be in Build Settings.")]
    [SerializeField] private string lobbySceneName = "ModeLobby";

    [Tooltip("The time in seconds before automatically redirecting.")]
    [SerializeField] private float redirectDelay = 10f;

    [Header("UI References")]
    [Tooltip("The button that allows the user to proceed immediately.")]
    [SerializeField] private Button proceedButton;

    [Tooltip("The Text component to display the countdown message.")]
    [SerializeField] private TextMeshProUGUI countdownText;

    private float _countdownTimer;
    private bool _isRedirecting = false; // A flag to prevent multiple loads

    private void Start()
    {
        // --- Setup Initial State ---
        _countdownTimer = redirectDelay;
        if (countdownText == null)
        {
            Debug.LogError("Countdown Text is not assigned!", this);
        }

        // --- Subscribe to Button Click Event ---
        if (proceedButton != null)
        {
            proceedButton.onClick.AddListener(ProceedToLobby);
        }
        else
        {
            Debug.LogError("Proceed Button is not assigned!", this);
        }
    }

    private void Update()
    {
        // --- Countdown Logic ---
        if (_countdownTimer > 0)
        {
            _countdownTimer -= Time.deltaTime;
            UpdateCountdownText();
        }
        else if (!_isRedirecting)
        {
            // Time is up, redirect automatically
            ProceedToLobby();
        }
    }

    /// <summary>
    /// Updates the UI text to show the remaining time.
    /// </summary>
    private void UpdateCountdownText()
    {
        if (countdownText != null)
        {
            // Using Mathf.CeilToInt to show whole numbers like 10, 9, 8...
            countdownText.text = $"Back to menu in {Mathf.CeilToInt(_countdownTimer)}...";
        }
    }

    /// <summary>
    /// The single method responsible for changing the scene.
    /// Can be called by the button or by the timer.
    /// </summary>
    public void ProceedToLobby()
    {
        // Prevent this from running multiple times if the button is spammed or timer fires simultaneously
        if (_isRedirecting)
        {
            return;
        }
        _isRedirecting = true;

        Debug.Log($"Redirecting to scene: {lobbySceneName}");

        // Unsubscribe from event to be safe
        proceedButton.onClick.RemoveListener(ProceedToLobby);

        SceneManager.LoadScene(lobbySceneName);
    }

    private void OnDestroy()
    {
        // Ensure we unsubscribe if the object is destroyed for any other reason
        if (proceedButton != null)
        {
            proceedButton.onClick.RemoveListener(ProceedToLobby);
        }
    }
}
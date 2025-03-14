using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerNameInput; // Input สำหรับชื่อผู้เล่น
    [SerializeField] private Button loginButton;        // ปุ่ม Login

    public static string PlayerName { get; private set; } 

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    private void OnLoginButtonClicked()
    {
        if (!string.IsNullOrEmpty(playerNameInput.text))
        {
            PlayerName = playerNameInput.text;
            Debug.Log($"Login Successful! Player Name: {PlayerName}");
            UnityEngine.SceneManagement.SceneManager.LoadScene("CreateAndJoin");
        }
        else
        {
            Debug.LogWarning("Please enter a player name.");
        }
    }
}


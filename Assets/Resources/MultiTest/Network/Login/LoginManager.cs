using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private Color NameEmpty;
    [SerializeField] private Color NameFill;

    [SerializeField] private TMP_InputField playerNameInput; // Input สำหรับชื่อผู้เล่น
    [SerializeField] private GameObject loginButtonObject;

    public string inputId = "myWebGLInput";
    private UnityEngine.UI.Button loginButton;
    private Image image_loginButton;
    public static string PlayerName { get; private set; } 

    private void Start()
    {
        loginButton = loginButtonObject.GetComponent<UnityEngine.UI.Button>();
        image_loginButton = loginButtonObject.GetComponent<Image>();

        loginButton.onClick.AddListener(OnLoginButtonClicked);

        playerNameInput.onValueChanged.AddListener(OnValueChanged);
        image_loginButton.color = NameEmpty;
    }
    private void OnValueChanged(string value)
    {
        if (!string.IsNullOrEmpty(playerNameInput.text))
        {
            image_loginButton.color = NameFill;
        }
        else
        {
            image_loginButton.color = NameEmpty;
        }
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


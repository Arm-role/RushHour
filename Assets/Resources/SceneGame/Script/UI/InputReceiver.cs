using UnityEngine;
using TMPro;

public class InputReceiver : MonoBehaviour
{
    public TMP_InputField inputField; // เชื่อมกับ Inspector

    // เรียกจาก JS ผ่าน SendMessage
    public void OnInputSubmit(string text)
    {
        inputField.text = text;
        Debug.Log("Input received from WebGL: " + text);
    }
}

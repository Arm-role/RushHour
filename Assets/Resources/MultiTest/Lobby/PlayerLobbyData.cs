using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLobbyData : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            if (NameText != null)
            {
                NameText.text = _name;
            }
        }
    }

    public Image _image;
    private bool _IsReady;
    public bool IsReady
    {
        get
        {
            return _IsReady;
        }
        set
        {
            _IsReady = value;
            if(_image != null)
            {
                if(_IsReady)
                {
                    _image.color = Color.green;
                }
                else
                {
                    _image.color = Color.red;
                }
            }
        }
    }
    public PlayerRef player { get; set; }
    public GameObject Object;
    public void GetReady(bool isReady)
    {
        IsReady = isReady;
    }
}

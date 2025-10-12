using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Button _createButton;
    [SerializeField] private Button _joinButton;

    void Start()
    {
        _createButton.onClick.AddListener(() => UIManager.Instance.SetCreateRoom(true));
        _joinButton.onClick.AddListener(() => UIManager.Instance.SetCreateRoom(false));
    }
}
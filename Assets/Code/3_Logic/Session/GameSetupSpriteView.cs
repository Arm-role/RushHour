using UnityEngine;
using SessionEvent;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GameSetupSpriteView : MonoBehaviour
{
    [SerializeField] private Sprite[] _keySprite;
    [SerializeField] private KeyImages[] _keyImage;
    [SerializeField] private Image _iconImage;


    [SerializeField] private Color _defaultKeyColor;
    [SerializeField] private Color _selectedKeyColor;

    public void Start()
    {
        SortKeySprite();
    }

    private void SortKeySprite()
    {
        for (int i = 0; i < _keyImage.Length; i++)
        {
            _keyImage[i].KeyImage.sprite = _keySprite[i];
        }
    }
    public void SetIconImage(int iconIndex)
    {
        _iconImage.sprite = _keySprite[iconIndex];
    }
    public void SelectedKey(int key)
    {
        _keyImage[key].BGImage.color = _selectedKeyColor;
    }
    public void RejectedKey(int key)
    {
        _keyImage[key].BGImage.color = _defaultKeyColor;
        Debug.Log($"Reject {key}");
    }

    public void ResetKey(List<int> keys)
    {
        foreach (var key in keys)
        {
            RejectedKey(key);
        }
    }

    public void RollKey(List<int> keys)
    {
        foreach (var key in keys)
        {
            SelectedKey(key);
        }
    }
}

[Serializable]
public class KeyImages
{
    public Image KeyImage;
    public Image BGImage;
}
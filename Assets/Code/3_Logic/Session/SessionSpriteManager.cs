using UnityEngine;
using SessionEvent;
using UnityEngine.UI;
using System.Collections.Generic;

public class SessionSpriteManager : MonoBehaviour
{
    [SerializeField] private Sprite[] _keySprite;
    [SerializeField] private Image[] _keyImage;

    [SerializeField] private Sprite _codeSprite;
    [SerializeField] private Image[] _codeImage1;
    [SerializeField] private Image[] _codeImage2;


    private Dictionary<int, Sprite> _sprites = new Dictionary<int, Sprite>();

    public void Start()
    {
        for (int i = 0; i < _keySprite.Length; i++)
        {
            _sprites[i] = _keySprite[i];
        }

        SortKeySprite();

        EventManager.Subscribe<CodeSession>(SetCode);
        EventManager.Subscribe<ResetCode>(ResetKey);
    }

    private void SortKeySprite()
    {
        for (int i = 0; i < _keyImage.Length; i++)
        {
            _keyImage[i].sprite = _sprites[i];
        }
    }

    private void SetCode(CodeSession keySession)
    {
        if (!_codeImage1[keySession.Pos]) return;

        var codeImage = _codeImage1[keySession.Pos];
        codeImage.sprite = _sprites[keySession.Key];

        if (!_codeImage2[keySession.Pos]) return;

        var codeImage2 = _codeImage2[keySession.Pos];
        codeImage2.sprite = _sprites[keySession.Key];
    }

    private void ResetKey(ResetCode reset)
    {
        for (int i = 0; i < _codeImage1.Length; i++)
        {
            var codeImage = _codeImage1[i];
            codeImage.sprite = _codeSprite;
        }
        for (int i = 0; i < _codeImage2.Length; i++)
        {
            var codeImage = _codeImage2[i];
            codeImage.sprite = _codeSprite;
        }
    }
}

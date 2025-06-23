//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//using System;
//using DG.Tweening;

//public class ItemLabelData : MonoBehaviour, IPoolable
//{
//    [SerializeField] private Image iconImage;
//    [SerializeField] private TextMeshProUGUI amountText;
//    [SerializeField] private Canvas canvas;
//    [SerializeField] private Image completeImage;
//    [SerializeField] private Image backgroundImage;
//    [SerializeField] private Color missionColor;
//    [SerializeField] private Color completeColor;

//    private int _amount;
//    private int _orderInLayer;
//    private Vector2 _target;
//    private float _moveDuration = 0.25f;
//    private bool _isComplete = false;
//    private Tween _moveTween;
//    private MenuListPool pool;

//    private Sprite _iconSprite;
//    public Sprite IconSprite
//    {
//        get => _iconSprite;
//        set
//        {
//            _iconSprite = value;
//            iconImage.sprite = value;
//        }
//    }


//    public int Amount 
//    {
//        get => _amount;
//        set
//        {
//            _amount = value;
//            if (_amount <= 0) Complete();
//            else amountText.text = $"X{ _amount}";
//        }
//    }


//    public int OrderInLayer
//    {
//        get => _orderInLayer;
//        set
//        {
//            _orderInLayer = value;
//            canvas.sortingOrder = value;
//        }
//    }

//    public Vector2 Target
//    {
//        get => _target;
//        set
//        {
//            _target = value;
//            MoveToTaget(_target);
//        }
//    }

//    private void MoveToTaget(Vector2 _target)
//    {
//        _moveTween?.Kill();
//        _moveTween = transform.DOMove(_target, _moveDuration).SetEase(Ease.OutQuad);
//    }

//    public void Init(MenuListPool pool) => this.pool = pool;
//    public void OnSpawned()
//    {
//        gameObject.SetActive(true);
//        ResetState();
//    }

//    public void OnDespawned()
//    {
//        pool.Return(this);
//        gameObject.SetActive(false);
//        _moveTween?.Kill();
//    }
//    private void ResetState()
//    {
//        _isComplete = false;
//        backgroundImage.color = missionColor;
//        amountText.gameObject.SetActive(true);
//        completeImage.gameObject.SetActive(false);
//        _moveTween?.Kill();
//    }
//    public void Setup(ItemLabelData data)
//    {
//        IconSprite = data.IconSprite;
//        Amount = data.Amount;

//        ResetState();
//    }
//    public void DataUpdate(int ammount, Vector2 taget)
//    {
//        if (_isComplete) return;

//        SetSiblingIndexFirst();

//        Amount  -= ammount;
//        Target = taget;
//    }
//    private void Complete()
//    {
//        amountText.text = "X0";
//        _isComplete = true;

//        amountText.gameObject.SetActive(false);
//        completeImage.gameObject.SetActive(true);

//        backgroundImage.DOColor(completeColor, 0.3f).SetEase(Ease.InOutSine);

//        SetSiblingIndexLast();

//        GridLayoutItemLabel.OnComplete?.Invoke();
//    }
//    private void SetSiblingIndexFirst()
//    {
//        transform.SetSiblingIndex(0);
//        GridLayoutItemLabel.OnDataUpdate?.Invoke(this);
//    }
//    private void SetSiblingIndexLast()
//    {
//        int index = transform.parent.childCount - 1;
//        transform.SetSiblingIndex(index);
//        GridLayoutItemLabel.OnDataUpdate?.Invoke(this);
//    }
//}
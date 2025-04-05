using TMPro;
using UnityEngine;

public class Request : MonoBehaviour
{
    public SpriteRenderer Renderer;
    public TextMeshProUGUI one;
    public TextMeshProUGUI two;
    public GameObject display;
    public GameObject sever;

    private void Start()
    {
        ItemEvents.Instance.OnFoodDisplay.Subscribe(SetDisplay);
        ItemEvents.Instance.OnNextIngredient.Subscribe(SetState);
    }
    private void OnDestroy()
    {
        if (ItemEvents.Instance != null)
        {
            ItemEvents.Instance.OnFoodDisplay.UnSubscribe(SetDisplay);
            ItemEvents.Instance.OnNextIngredient.UnSubscribe(SetState);
        }
    }
    public void SetState((Sprite sprite, int amount, int limmit) item)
    {
        SetDisplay(false);
        Renderer.sprite = item.sprite;
        one.text = item.amount.ToString();
        two.text = item.limmit.ToString();

    }
    public void SetDisplay(bool displayed)
    {
        display.SetActive(!displayed);
        sever.SetActive(displayed);
    }
}

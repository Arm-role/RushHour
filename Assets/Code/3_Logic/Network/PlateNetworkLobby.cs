using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlateNetworkLobby : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(OnVisibilityChanged))]
    public bool IsActive { get; set; }

    [Networked, OnChangedRender(nameof(OnChangeFoodSprite))]
    public int foodSpriteId { get; set; }

    [SerializeField] private GameObject textureObject;
    [SerializeField] private Image foodImage;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Sprite[] foodSprites;
    private void OnVisibilityChanged()
    {
        textureObject.SetActive(IsActive);
    }
    private void OnChangeFoodSprite()
    {
        foodImage.sprite = foodSprites[foodSpriteId];
        text.text = foodSpriteId.ToString();
    }
    public override void Spawned()
    {
        textureObject.SetActive(IsActive);
    }
}
using UnityEngine;
using UnityEngine.UI;

public abstract class KitchenItem : MonoBehaviour
{
    public Transform child;
    protected float _timer;
    public virtual float Timer { get; set; }
    public Slider slider;

    protected ItemRendering itemRendering;

    protected virtual void Start()
    {
        itemRendering = GetComponent<ItemRendering>();
    }

    protected virtual void Update() { }

    public GameObject CreateOnPlate(GameObject OB, bool isRandom)
    {
        GameObject ob = Instantiate(OB, child.position, child.rotation, child);

        if (isRandom)
        {
            float randomZ = UnityEngine.Random.Range(0f, 360f);

            ob.transform.rotation = Quaternion.Euler(
            ob.transform.rotation.eulerAngles.x,
            ob.transform.rotation.eulerAngles.y,
            randomZ);
        }

        var renderer = ob.GetComponent<SpriteRenderer>();
        var rendererRef = OB.GetComponent<SpriteRenderer>();

        renderer.sortingOrder = rendererRef.sortingOrder + 1;

        itemRendering.AddRenderor(renderer);
        itemRendering.AddSortingOrder(rendererRef.sortingOrder + 1);

        return ob;
    }

    public virtual void PickUpItem(ItemHandle Handle) { }
    public virtual void DropItem() { }

    public bool TouchOverMe(GameObject obj)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(InputHandle.GetTouchPosition());
        Collider2D coll = Physics2D.OverlapPoint(new Vector2(mouseWorldPos.x, mouseWorldPos.y));

        return coll.gameObject == obj;
    }
}

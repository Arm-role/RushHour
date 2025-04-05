using UnityEngine;

public class TrashBox : MonoBehaviour
{
    public GameObject texture;
    private Collider2D collider2d;
    private void Start()
    {
        collider2d = GetComponent<Collider2D>();

        GameEvents.Instance.OnTouchItem.Subscribe(GetDelegate);
        GetDelegate(false);
    }
    private void OnDestroy()
    {
        GameEvents.Instance.OnTouchItem.UnSubscribe(GetDelegate);
    }
    public void GetDelegate(bool isTouch)
    {
        //Debug.Log(isTouch);
        if (collider2d != null && texture != null)
        {
            texture.SetActive(isTouch);
            collider2d.enabled = isTouch;
        }
    }
}

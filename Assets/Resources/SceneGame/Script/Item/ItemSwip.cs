using UnityEngine;

public class ItemSwip : MonoBehaviour
{
    [SerializeField]
    private float swipt_Offet;

    private bool isSwip = false;
    private float oldPosition;
    private Rigidbody2D rb;
    [SerializeField]
    private float Force;
    [SerializeField]
    private float Offset;

    public delegate void isDrager();
    public event isDrager OnDrager;

    private float screenWidth = Screen.width;
    private float screenHeight = Screen.height;

    private Vector3 bottomEdge;
    private Vector3 topEdge;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Camera cam = Camera.main;

        // เปลี่ยนขอบล่างและขอบบนแทนซ้ายและขวา
        bottomEdge = cam.ScreenToWorldPoint(new Vector3(screenWidth / 2, 0, cam.nearClipPlane));
        topEdge = cam.ScreenToWorldPoint(new Vector3(screenWidth / 2, screenHeight, cam.nearClipPlane));
    }

    private void Update()
    {
        if (transform.position.y <= (bottomEdge.y + swipt_Offet) - Offset || transform.position.y >= (topEdge.y - swipt_Offet) + Offset)
        {
            OnSwipt();
        }
    }

    public void OnSwipt()
    {
        if (!isSwip)
        {

            if (transform.position.y <= bottomEdge.y + swipt_Offet || transform.position.y >= topEdge.y - swipt_Offet)
            {
                oldPosition = transform.position.x;
                transform.position = new Vector2(-transform.position.x, transform.position.y);
                isSwip = true;
                ParticleManager.instance.CreateParticle("SmokeParticle", transform.position);
                OnDrager?.Invoke();
            }
        }

        if (isSwip)
        {
            if (oldPosition != transform.position.x)
            {
                if (transform.position.y <= bottomEdge.y + swipt_Offet)
                {
                    rb.AddForce(Vector2.up * Force);
                }
                else if (transform.position.y >= topEdge.y - swipt_Offet)
                {
                    rb.AddForce(Vector2.down * Force);
                }
            }
            if (rb.velocity.magnitude < 2f)
            {
                oldPosition = transform.position.x;
                isSwip = false;
            }
        }
    }

    public void Ontap(bool tap)
    {
        if (tap)
        {
            oldPosition = transform.position.x;
            isSwip = false;
        }
    }
}

using UnityEngine;

[ExecuteInEditMode] // ทำให้โค้ดทำงานใน Edit Mode
public class GridLayoutManager : MonoBehaviour
{
    public int rows = 3; // จำนวนแถว
    public int columns = 3; // จำนวนคอลัมน์
    public Vector2 spacing = new Vector2(1.5f, 1.5f); // ระยะห่างระหว่างออบเจ็กต์
    public bool autoArrange = false; // เปิด/ปิด Auto Arrange ใน Inspector

    private void Update()
    {
        if (!Application.isPlaying && autoArrange)
        {
            ArrangeChildren();
            autoArrange = false;
        }
    }

    public void ArrangeChildren()
    {
        int childCount = transform.childCount;
        if (childCount == 0)
        {
            Debug.LogWarning("ไม่มี Child Objects ให้จัดเรียง!");
            return;
        }

        for (int i = 0; i < childCount; i++)
        {
            int row = i / columns; // หาค่าของแถว
            int col = i % columns; // หาค่าของคอลัมน์
            Vector3 position = new Vector3(col * spacing.x, -row * spacing.y, 0);
            transform.GetChild(i).localPosition = position;
        }
    }
}

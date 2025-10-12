using UnityEngine;
using UnityEngine.UI;

public class CenteredSlider : Slider
{
    protected override void Set(float input, bool sendCallback)
    {
        base.Set(input, sendCallback);
        UpdateFill();
    }

    private void UpdateFill()
    {
        if (fillRect == null) return;

        var fill = fillRect as RectTransform;
        if (fill == null) return;

        float halfWidth = (GetComponent<RectTransform>().rect.width / 2f);

        // ขยาย Fill ออกจาก Center
        fill.sizeDelta = new Vector2(value * halfWidth * 2f, 0f);
    }
}

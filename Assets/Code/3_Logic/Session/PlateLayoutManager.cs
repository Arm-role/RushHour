using UnityEngine;
using Fusion;
using System.Collections.Generic;

public class PlateLayoutManager : MonoBehaviour
{
    [SerializeField] private float _layoutRadius = 250f;
    private RectTransform _containerRect;

    private void Awake()
    {
        _containerRect = GetComponent<RectTransform>();
    }
    public void ArrangePlates(IList<NetworkObject> activePlates)
    {
        int plateCount = activePlates.Count;

        foreach (var plateNO in activePlates)
        {
            if (plateNO == null) continue;
            plateNO.GetComponent<RectTransform>().SetParent(_containerRect, false);
        }

        if (plateCount == 0) return;

        for (int i = 0; i < plateCount; i++)
        {
            RectTransform plateRect = activePlates[i].GetComponent<RectTransform>();

            float angle = i * (360f / plateCount) * -1f;
            float angleRad = angle * Mathf.Deg2Rad;
            float x = _layoutRadius * Mathf.Cos(angleRad);
            float y = _layoutRadius * Mathf.Sin(angleRad);
            plateRect.anchoredPosition = new Vector2(x, y);
        }
    }
}
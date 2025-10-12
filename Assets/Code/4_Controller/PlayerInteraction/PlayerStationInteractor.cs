using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStationInteractor : MonoBehaviour
{
    private Station _nearbyStation;
    void Update()
    {
        if (InputHandle.GetDoubleTouch())
        {
            Vector3 touchPos = InputHandle.GetTouchPosition();
            Collider2D hit = Physics2D.OverlapPoint(GetTouchPos(new Vector2(touchPos.x, touchPos.y)));

            if (hit != null)
            {
                if (hit.TryGetComponent(out _nearbyStation))
                {
                    if (_nearbyStation.StationData is ToolWorkData data && data.IsWorking)
                    {
                        _nearbyStation.worker.ReceiveExternalInput();
                    }
                }
            }
        }
        else if (InputHandle.GetUndo(out var touchPos))
        {
            Collider2D hit = Physics2D.OverlapPoint(GetTouchPos(touchPos));

            if (hit != null)
            {
                if (hit.TryGetComponent(out _nearbyStation))
                {
                    _nearbyStation.Interact();
                }
            }
        }
    }
    private Vector2 GetTouchPos(Vector3 touchPos)
    {
        Vector3 posWorld = Camera.main.ScreenToWorldPoint(touchPos);
        return new Vector2(posWorld.x, posWorld.y);
    }
}

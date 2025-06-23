using System.Collections;
using UnityEngine;

public class PlayerStationInteractor : MonoBehaviour
{
    private Station _nearbyStation;
    void Update()
    {
        if (InputHandle.GetUndo(out Vector3 touchPos))
        {
            Collider2D hit = Physics2D.OverlapPoint(GetTouchPos(touchPos));

            if (hit != null)
            {
                _nearbyStation = hit.GetComponent<Station>();

                if (_nearbyStation.stationData.IsWorking)
                {
                    _nearbyStation.worker.ReceiveExternalInput();
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

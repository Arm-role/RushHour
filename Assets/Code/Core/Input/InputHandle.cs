using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandle
{
    /// <summary>
    /// ตรวจสอบว่ามีการคลิกเมาส์ซ้ายหรือการปล่อยนิ้วจากการแตะหน้าจอ
    /// </summary>
    public static bool GetInputButtonUp()
    {
        if (Input.GetMouseButtonUp(0))
            return true;

        // ตรวจสอบการปล่อยนิ้วจากหน้าจอสัมผัส
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            return true;

        return false;
    }

    /// <summary>
    /// ตรวจสอบว่ากำลังคลิกเมาส์ซ้ายหรือแตะหน้าจออยู่
    /// </summary>
    public static bool GetInputButton()
    {
        if (Input.GetMouseButton(0))
            return true;

        // ตรวจสอบว่ากำลังแตะหน้าจออยู่
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            return true;

        return false;
    }

    /// <summary>
    /// ตรวจสอบว่ามีการคลิกเมาส์ซ้ายหรือแตะหน้าจอครั้งเดียว
    /// </summary>
    public static bool GetInputButtonDown()
    {
        if (Input.GetMouseButtonDown(0))
            return true;

        // ตรวจสอบว่ามีการแตะหน้าจอครั้งเดียว
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            return true;

        return false;
    }

    public static Vector3 GetTouchPosition()
    {
        if (Input.touchCount > 0)
            return Input.GetTouch(0).position;

        return Input.mousePosition;
    }

    private static float lastTapTime = 0.1f;
    private const float doubleTapThreshold = 0.3f;
    /// <summary>
    /// ตรวจสอบการคลิกขวาหรือแตะหน้าจอสองครั้ง
    /// </summary>
    /// <returns>True หากตรวจพบการคลิกขวาหรือแตะหน้าจอสองครั้ง</returns>
    public static bool GetDoubleTouch()
    {
        if (Input.GetMouseButtonDown(1))
            return true;


        if (Input.touchCount > 0 && Input.GetTouch(0).tapCount == 2)
            return true;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                if (Time.deltaTime - lastTapTime <= doubleTapThreshold)
                {
                    lastTapTime = 0f;
                    return true;
                }

                lastTapTime = Time.deltaTime;
            }
        }
        return false;
    }
    /// <summary>
    /// ตรวจสอบว่ามีการคลิกเมาส์กลางหรือแช่หน้าจอและแตะหน้าจอ
    /// </summary>
    public static bool GetUndo()
    {
        if (Input.GetMouseButtonDown(2))
        {
            return true;
        }

        if (Input.touchCount < 2)
        {
            return false;
        }

        Touch firstTouch = Input.GetTouch(0);
        Touch secondTouch = Input.GetTouch(1);

        bool isFirstTouch = firstTouch.phase == TouchPhase.Stationary;
        bool isSecorndTouch = secondTouch.phase == TouchPhase.Began;


        if (isFirstTouch && isSecorndTouch)
        {
            return true;
        }

        return false;
    }
    public static bool GetUndo(out Vector3 TouchPos)
    {
        TouchPos = Vector3.zero;

        if (Input.GetMouseButtonDown(2))
        {
            TouchPos = Input.mousePosition;
            return true;
        }

        if (Input.touchCount < 2)
        {
            return false;
        }

        Touch firstTouch = Input.GetTouch(0);
        Touch secondTouch = Input.GetTouch(1);

        bool isFirstTouch = firstTouch.phase == TouchPhase.Stationary;
        bool isSecorndTouch = secondTouch.phase == TouchPhase.Began;

        if (isFirstTouch && isSecorndTouch)
        {
            TouchPos = Input.GetTouch(0).position;
            return true;
        }

        return false;
    }


    public static bool EnterButton()
    {
        if (Input.GetKey(KeyCode.KeypadEnter)) return true;

        return false;
    }
}

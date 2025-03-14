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
        // ตรวจสอบเมาส์ซ้ายปล่อยคลิก
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
        // ตรวจสอบว่ากำลังกดเมาส์ซ้ายอยู่
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
        // ตรวจสอบว่ามีการกดเมาส์ซ้ายครั้งเดียว
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
}

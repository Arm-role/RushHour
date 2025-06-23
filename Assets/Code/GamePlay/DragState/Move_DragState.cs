using UnityEngine;

public class Move_DragState : IDrag // ลาก
{
    public void Enter(DragManager dragManager)
    {
        dragManager.holdTimer = 0f;
        dragManager.lastTouchPos = dragManager.GetTouchPos();
        dragManager.hasMovedTooMuch = false;

        dragManager.EnterNotify(this);
    }

    public void Execute(DragManager dragManager)
    {
        if (InputHandle.GetInputButton())
        {
            Vector2 currentPos = dragManager.GetTouchPos();

            // ตรวจจับว่า "ขยับเกิน" หรือยัง
            if (!dragManager.hasMovedTooMuch &&
                 Vector2.Distance(currentPos, dragManager.lastTouchPos) > dragManager.holdMoveTolerance)
            {
                dragManager.hasMovedTooMuch = true;
            }

            // ขยับตำแหน่งตามปกติ
            dragManager.ExcuteNotify(this);

            // ถ้า "ยังไม่ขยับ" → เพิ่ม hold timer
            if (!dragManager.hasMovedTooMuch)
            {
                dragManager.holdTimer += Time.deltaTime;

                if (dragManager.holdTimer >= dragManager.holdThreshold)
                {
                    dragManager.SetState(new Hold_DragState());
                    return;
                }
            }
        }
        else if (InputHandle.GetInputButtonUp())
        {
            dragManager.SetState(new Release_DragState());
        }
    }

    public void Exit(DragManager dragManager)
    {
        dragManager.holdTimer = 0f;
        dragManager.ExitNotify(this);
    }
}

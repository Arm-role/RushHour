using UnityEngine;

public class Idle_DragState : IDrag //อยู่นิ่งๆ
{
    public void Enter(DragManager dragManager)
    {
        dragManager.ClearItem();
        dragManager.EnterNotify(this);
    }

    public void Execute(DragManager dragManager)
    {
        if (InputHandle.GetInputButtonDown())
        {
            Collider2D hitCollider = Physics2D.OverlapPoint(dragManager.GetTouchPos());

            if (hitCollider != null && hitCollider.CompareTag("Dragable"))
            {
                dragManager.SetItem(hitCollider);
                dragManager.SetState(new Grabbed_DragState());
            }
        }
        dragManager.ExcuteNotify(this);
    }

    public void Exit(DragManager dragManager)
    {
        dragManager.ExitNotify(this);
    }
}

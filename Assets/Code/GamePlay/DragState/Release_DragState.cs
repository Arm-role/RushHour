using UnityEngine;

public class Release_DragState : IDrag //ปล่อย
{
    public void Enter(DragManager dragManager)
    {
        dragManager.EnterNotify(this);
    }

    public void Execute(DragManager dragManager)
    {
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(dragManager.GetTouchPos());

        if (hitColliders.Length > 0)
        {
            dragManager.ExcuteNotify(this);

            bool foundOther = false;
            foreach (Collider2D collider in hitColliders)
            {
                if (collider.gameObject != dragManager.currentItem)
                {
                    foundOther = true;
                    dragManager.SetState(new Dropped_DragState());
                }
            }
            if (!foundOther)
            {
                dragManager.SetState(new Idle_DragState());
            }
        }
        else
        {
            dragManager.SetState(new Idle_DragState());
        }
    }

    public void Exit(DragManager dragManager)
    {
        dragManager.ExitNotify(this);
    }
}

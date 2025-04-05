using UnityEngine;

public class Idle_DragState : IDrag
{
    public void Enter(DragManager dragManager)
    {
        dragManager.SetTouchItem(false);
        dragManager.ClearItem();
    }

    public void Execute(DragManager dragManager)
    {
        if (InputHandle.GetInputButtonDown())
        {
            Collider2D hitCollider = Physics2D.OverlapPoint(dragManager.GetTouchPos());

            if (hitCollider != null && hitCollider.CompareTag("Item"))
            {
                dragManager.SetItem(hitCollider);
                dragManager.SetState(new Grabbed_DragState());
            }
        }
    }

    public void Exit(DragManager dragManager) { }
}
public class Grabbed_DragState : IDrag
{
    public void Enter(DragManager dragManager)
    {
        dragManager.TapItem(true);
        dragManager.SetTouchItem(true);
    }

    public void Execute(DragManager dragManager)
    {
        if (InputHandle.GetInputButton())
        {
            dragManager.MoveItem(dragManager.GetTouchPos());
            dragManager.SetState(new Move_DragState());
        }
    }

    public void Exit(DragManager dragManager)
    {
        dragManager.TapItem(false);
    }
}
public class Move_DragState : IDrag
{
    public void Enter(DragManager dragManager)
    {
        dragManager.HoldItem(true);
    }

    public void Execute(DragManager dragManager)
    {
        if (InputHandle.GetInputButton())
        {
            dragManager.MoveItem(dragManager.GetTouchPos());
        }
        else if (InputHandle.GetInputButtonUp())
        {
            dragManager.SetState(new Release_DragState());
        }
    }

    public void Exit(DragManager dragManager)
    {
        dragManager.HoldItem(false);
    }
}
public class Release_DragState : IDrag
{
    public void Enter(DragManager dragManager) { }

    public void Execute(DragManager dragManager)
    {
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(dragManager.GetTouchPos());

        if (hitColliders.Length > 0)
        {
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
    }

    public void Exit(DragManager dragManager) { }
}
public class Dropped_DragState : IDrag
{
    public void Enter(DragManager dragManager) { }
    public void Execute(DragManager dragManager)
    {
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(dragManager.GetTouchPos());

        if (hitColliders.Length > 0)
        {
            EKitchenType currentType = dragManager.currentType;

            foreach (Collider2D collider in hitColliders)
            {
                if (collider.TryGetComponent<ItemHandle>(out var handle))
                {
                    if (handle.kitchenType == EKitchenType.Tool && currentType == EKitchenType.Food)
                    {
                        dragManager.ItemOn(new OnTool(collider));
                    }
                    else if (handle.kitchenType == EKitchenType.Ware && currentType == EKitchenType.Food)
                    {
                        dragManager.ItemOn(new OnWare(collider));
                    }
                }
                else
                {
                    if (collider.CompareTag("TrashBox") && currentType == EKitchenType.Food)
                    {
                        dragManager.ItemOn(new ItemOnTrash());
                    }
                    else if (collider.CompareTag("ArrowLeft") && currentType == EKitchenType.Food)
                    {
                        dragManager.ItemOn(new OnArrowLeft());
                    }
                    else if (collider.CompareTag("ArrowRight") && currentType == EKitchenType.Food)
                    {
                        dragManager.ItemOn(new OnArrowRight());
                    }
                    else if (collider.CompareTag("TrashBox") && currentType == EKitchenType.Ware)
                    {
                        dragManager.ItemOn(new PlateOnTrash());
                    }
                    else if (collider.CompareTag("Counter") && currentType == EKitchenType.Ware)
                    {
                        dragManager.ItemOn(new OnCounter(collider));
                    }
                }
            }
        }
        dragManager.SetState(new Idle_DragState());
    }

    public void Exit(DragManager dragManager) { }
}

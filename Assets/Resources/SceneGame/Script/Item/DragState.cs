using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle_DragState : IDrag
{
    public void Enter(ItemDrag itemDrag)
    {
        itemDrag.SetTouchItem(false);
    }

    public void Execute(ItemDrag itemDrag)
    {
        if (InputHandle.GetInputButtonDown())
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(InputHandle.GetTouchPosition());
            Vector2 mousePosition2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition2D);
            if (hitCollider != null && hitCollider.gameObject == itemDrag.gameObject)
            {
                itemDrag.SetState(new Grabbed_DragState());
            }
        }
    }

    public void Exit(ItemDrag itemDrag) { }
}
public class Grabbed_DragState : IDrag
{
    public void Enter(ItemDrag itemDrag)
    {
        itemDrag.SetTap(true);
        itemDrag.SetTouchItem(true);
    }

    public void Execute(ItemDrag itemDrag)
    {
        if (InputHandle.GetInputButton())
        {
            Vector3 touchPos = itemDrag.GetTouchWorldPos(InputHandle.GetTouchPosition());
            itemDrag.MoveTo(touchPos);
            itemDrag.SetState(new Move_DragState());
        }
    }

    public void Exit(ItemDrag itemDrag)
    {
        itemDrag.SetTap(false);
    }
}
public class Move_DragState : IDrag
{
    public void Enter(ItemDrag itemDrag)
    {
        itemDrag.SetHolder(true);
    }

    public void Execute(ItemDrag itemDrag)
    {
        if (InputHandle.GetInputButton()) // Continue dragging
        {
            Vector3 touchPos = itemDrag.GetTouchWorldPos(Input.mousePosition);
            itemDrag.MoveTo(touchPos);
        }
        else if (InputHandle.GetInputButtonUp()) // When the drag is released
        {
            itemDrag.SetState(new Release_DragState());
        }
    }

    public void Exit(ItemDrag itemDrag)
    {
        itemDrag.SetHolder(false); // Notify that the object is no longer "held"
    }
}
public class Release_DragState : IDrag
{
    public void Enter(ItemDrag itemDrag)
    {
    }

    public void Execute(ItemDrag itemDrag)
    {

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePosition2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition2D);
        if (hitColliders.Length > 0)
        {
            bool foundOther = false;

            foreach (Collider2D collider in hitColliders)
            {
                if (collider.gameObject != itemDrag.gameObject)
                {
                    foundOther = true;
                    itemDrag.SetState(new Dropped_DragState());
                }
            }
            if (!foundOther)
            {
                itemDrag.SetState(new Idle_DragState());
            }

        }
    }

    public void Exit(ItemDrag itemDrag) { }
}
public class Invalid_DragState : IDrag
{
    public void Enter(ItemDrag itemDrag)
    {
    }

    public void Execute(ItemDrag itemDrag)
    {
        if (InputHandle.GetInputButtonUp())
        {
            itemDrag.SetState(new Idle_DragState());
        }
    }

    public void Exit(ItemDrag itemDrag)
    {
    }
}
public class Dropped_DragState : IDrag  
{
    public void Enter(ItemDrag itemDrag)
    {
    }

    public void Execute(ItemDrag itemDrag)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePosition2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition2D);
        if (hitColliders.Length > 0)
        {
            string myTag = itemDrag.transform.tag;
            foreach (Collider2D collider in hitColliders)
            {
                if (collider.CompareTag("TrashBox") && myTag == "item")
                {
                    itemDrag.ItemOn(new ItemOnTrash());
                }
                else if(collider.CompareTag("ArrowLeft") && myTag == "item")
                {
                    itemDrag.ItemOn(new OnArrowLeft());
                }
                else if(collider.CompareTag("ArrowRight") && myTag == "item")
                {
                    itemDrag.ItemOn(new OnArrowRight());
                }
                else if (collider.CompareTag("Tool") && myTag == "item")
                {
                    itemDrag.ItemOn(new OnTool(collider));
                }
                else if (collider.CompareTag("Ware") && myTag == "item")
                {
                    itemDrag.ItemOn(new OnWare(collider));
                }
                else if (collider.CompareTag("TrashBox") && myTag == "Ware")
                {
                    itemDrag.ItemOn(new PlateOnTrash());
                }
                else if (collider.CompareTag("Counter") && myTag == "Ware")
                {
                    itemDrag.ItemOn(new OnCounter(collider));
                }
            }
        }
        itemDrag.SetState(new Idle_DragState());
    }

    public void Exit(ItemDrag itemDrag)
    {
    }
}

public class Hold_DragState : IDrag
{
    public void Enter(DragManager dragManager)
    {
        dragManager.EnterNotify(this);
    }

    public void Execute(DragManager dragManager)
    {
        if (InputHandle.GetInputButton())
        {
            dragManager.ExcuteNotify(this);
        }
        else if (InputHandle.GetInputButtonUp())
        {
            dragManager.SetState(new Release_DragState());
        }
    }

    public void Exit(DragManager dragManager)
    {
        dragManager.ExitNotify(this);
    }
}

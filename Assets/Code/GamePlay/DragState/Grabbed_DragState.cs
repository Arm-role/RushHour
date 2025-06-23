public class Grabbed_DragState : IDrag //คลิก แตะ
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
            dragManager.SetState(new Move_DragState());
        }

    }

    public void Exit(DragManager dragManager)
    {
        dragManager.ExitNotify(this);
    }
}

public class Dropped_DragState : IDrag // ตกใส่เป้าหมาย
{
    public void Enter(DragManager dragManager)
    {
        dragManager.EnterNotify(this);
    }
    public void Execute(DragManager dragManager)
    {
        dragManager.ExcuteNotify(this);
        dragManager.SetState(new Idle_DragState());
    }

    public void Exit(DragManager dragManager)
    {
        dragManager.ExitNotify(this);
    }
}

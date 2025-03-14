public class GameManager_Setup : IGameLevel
{
    public void Enter(GameManager manager)
    {
    }
    public void Execute(GameManager manager)
    {
        if (EvenManager.IsReady)
        {
            manager.SetState(new Wait_For_NextLevel());
        }
    }
    public void Exit(GameManager manager)
    {
        Item plate = ScriptTable_Contain.instance.GetItem(EItem.Plate);
        Item pan = ScriptTable_Contain.instance.GetItem(EItem.Pan);
        Item cutboard = ScriptTable_Contain.instance.GetItem(EItem.CutBoard);

        SpawnManager.Instance.OnSpawnItem(plate);
        SpawnManager.Instance.OnSpawnItem(pan);
        SpawnManager.Instance.OnSpawnItem(cutboard);
    }

}
public class Operating_At_Level : IGameLevel
{
    public void Enter(GameManager manager)
    {

        manager.RunMenu(manager.currentIndex);
    }
    public void Execute(GameManager manager)
    {

    }
    public void Exit(GameManager manager)
    {
    }

}
public class Wait_For_NextLevel : IGameLevel
{
    public void Enter(GameManager manager)
    {
    }
    public void Execute(GameManager manager)
    {
        manager.SetState(new Operating_At_Level());
    }
    public void Exit(GameManager manager)
    {

    }

}

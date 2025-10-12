using UnityEngine;

public class UIManager : SingletonBase<UIManager>
{
    protected UIManager() { }
    public bool IsCreatingRoom { get; private set; }
    public void SetCreateRoom(bool isCreating)
    {
        IsCreatingRoom = isCreating;
    }
}

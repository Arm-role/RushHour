
public interface IDrag : IState<ItemDrag> { }
public interface IDropTo
{
    void ItemOn(ItemDrag itemDrag);
}
public interface IGameLevel : IState<GameManager> { }

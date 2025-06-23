using UnityEngine;

public abstract class Item : ScriptableObject, IIdentifiable
{
    [SerializeField] protected string _name;
    public string Name => _name;
    public int Amount = 1;

    public virtual EItemType itemType => EItemType.None;

    public Sprite sprite;
    private void OnValidate()
    {
        _name = name;
    }
}

using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField] protected string _name;
    public string Name => _name;

    public virtual EItemType itemType => EItemType.None;

    public Sprite sprite;
    public Vector3 localScale;
    private void OnValidate()
    {
        _name = name;
    }
}

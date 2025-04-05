using UnityEngine;

[CreateAssetMenu(fileName = "newItem", menuName = "CreateItem/NewItem")]
public class Item : ScriptableObject, IIdentifiable
{
    [SerializeField] private int id = -1;
    [SerializeField] private string _name;
    public int ID => id;
    public string Name => _name;
    public EKitchenType kitchenType;

    public GameObject prefab;
    public Sprite sprite;

    public FoodState[] foodState;
    private void OnValidate()
    {
        _name = name;

        if (id == -1)
        {
            id = IDManager.Instance.GenerateNewID<Item>();
        }
    }
    private void OnDisable()
    {
        //IDManager.Instance.RemoveID<Item>(id);
    }
}

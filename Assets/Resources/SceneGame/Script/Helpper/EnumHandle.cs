using System.Collections.Generic;

public readonly struct EItem
{
    private static readonly Dictionary<string, EItem> _items = new();

    public static readonly EItem Baesil = new("Baesil");
    public static readonly EItem Bread = new("Bread");
    public static readonly EItem BreadCook = new("BreadCook");
    public static readonly EItem Chilli = new("Chilli");
    public static readonly EItem CutBean = new("CutBean");
    public static readonly EItem CutBoard = new("CutBoard");
    public static readonly EItem Egg = new("Egg");
    public static readonly EItem EggStar = new("EggStar");
    public static readonly EItem FriedRice = new("FriedRice");
    public static readonly EItem FriedSalmon = new("FriedSalmon");
    public static readonly EItem FriedSausage = new("FriedSausage");
    public static readonly EItem FriedShrimp = new("FriedShrimp");
    public static readonly EItem LongBean = new("LongBean");
    public static readonly EItem Mashroom = new("Mashroom");
    public static readonly EItem MashroomSlide = new("MashroomSlide");
    public static readonly EItem Pan = new("Pan");
    public static readonly EItem Papaya = new("Papaya");
    public static readonly EItem PapayaSlide = new("PapayaSlide");
    public static readonly EItem Plate = new("Plate");
    public static readonly EItem Rice = new("Rice");
    public static readonly EItem Salmon = new("Salmon");
    public static readonly EItem SalmonSlide = new("SalmonSlide");
    public static readonly EItem Sausage = new("Sausage");
    public static readonly EItem Shrimp = new("Shrimp");
    public static readonly EItem Tometo = new("Tometo");
    public static readonly EItem TometoSlide = new("TometoSlide");
    public static readonly EItem WaterMelon = new("WaterMelon");
    public static readonly EItem WaterMelonSlide = new("WaterMelonSlide");
    public static readonly EItem Weed = new("Weed");

    public readonly string Name;
    private EItem(string name)
    {
        Name = name;
        _items[name] = this;
    }
    public override string ToString() => Name;
    public static bool IsVaild(string name) =>_items.ContainsKey(name);
    public static bool TryGet(string name, out EItem item) => _items.TryGetValue(name, out item);
    public static IReadOnlyCollection<EItem> GetAllItems() => _items.Values;
}
public enum EKitchenType
{
    Food,
    Ware,
    Tool,
}
public enum EToolType
{
    ToolFried, ToolCutted
}
public enum EGameScene
{
    None,
    Login,
    Lobby,
    Game,
}
public enum EGameState
{
    None,
    Start,
    Run,
    Pause,
    End
}
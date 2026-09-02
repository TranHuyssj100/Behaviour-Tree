public interface IItem : IItemData
{
    ItemType type { get; }

    void OnUse();
}

public enum ItemType
{
    None = 0,
    Food = 1,
    Drink = 2,
    Equipment = 3,
    Other = 4
}

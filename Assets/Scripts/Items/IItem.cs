public interface IItem : IItemData
{
    ItemType type { get; }

    void OnUse();
}

public enum ItemType
{
    None = 0,
}

public interface IShop
{
    ShopType ShopType { get; }
    bool IsUnlocked { get; }
    bool CanSell(ItemDefinitionSO item);
}

[System.Serializable]
public enum ShopType
{
    None = 0,
    Pizza = 1,
    Burger = 2,
    Salad = 3,
    Drink = 4,
    Other = 5
}

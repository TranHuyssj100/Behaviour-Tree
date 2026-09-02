public interface IProperty
{
    PropertyType PropertyType { get; }
    int CurrentLevel { get; }
    int MaxLevel { get; }
    bool IsUnlocked { get; }

    void Unlock();
    void Upgrade();
}

[System.Serializable]
public enum PropertyType
{
    None = 0,
    Shop = 1,
    Burger = 2,
    Salad = 3,
    Drink = 4,
    Other = 5,
}

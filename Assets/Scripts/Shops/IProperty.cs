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
    Building = 1,
}

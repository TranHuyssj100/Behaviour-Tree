public interface IGameModeRules
{
    string DisplayName { get; }
    int StartingMoney { get; }
    bool CanBuildShops { get; }
    float RevenueMultiplier { get; }
    float TimeLimitSeconds { get; }

    bool IsCompleted(PlayerProfile profile, float elapsedSeconds);
}

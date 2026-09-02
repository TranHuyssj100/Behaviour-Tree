public class TimeAttackRules : IGameModeRules
{
    public TimeAttackRules(int startingMoney = 1000, float timeLimitSeconds = 180f)
    {
        StartingMoney = startingMoney;
        TimeLimitSeconds = timeLimitSeconds;
    }

    public string DisplayName => "Time Attack";
    public int StartingMoney { get; }
    public bool CanBuildShops => false;
    public float RevenueMultiplier => 2f;
    public float TimeLimitSeconds { get; }

    public bool IsCompleted(PlayerProfile profile, float elapsedSeconds)
    {
        return elapsedSeconds >= TimeLimitSeconds;
    }
}

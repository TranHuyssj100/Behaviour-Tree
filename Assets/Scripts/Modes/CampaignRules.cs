public class CampaignRules : IGameModeRules
{
    readonly int targetMoney;

    public CampaignRules(int startingMoney = 500, int targetMoney = 10000)
    {
        StartingMoney = startingMoney;
        this.targetMoney = targetMoney;
    }

    public string DisplayName => "Campaign";
    public int StartingMoney { get; }
    public bool CanBuildShops => true;
    public float RevenueMultiplier => 1f;
    public float TimeLimitSeconds => 0f;

    public bool IsCompleted(PlayerProfile profile, float elapsedSeconds)
    {
        return profile != null && profile.Money >= targetMoney;
    }
}

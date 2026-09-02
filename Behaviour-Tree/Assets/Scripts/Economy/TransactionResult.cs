public enum TransactionStatus
{
    Success = 0,
    InvalidRequest = 1,
    NotEnoughMoney = 2,
    ShopLocked = 3,
    ItemNotSold = 4,
    MaxLevelReached = 5,
    NotAllowedInMode = 6,
}

public readonly struct TransactionResult
{
    TransactionResult(TransactionStatus status, int amount)
    {
        Status = status;
        Amount = amount;
    }

    public TransactionStatus Status { get; }
    public int Amount { get; }
    public bool IsSuccess => Status == TransactionStatus.Success;

    public static TransactionResult Ok(int amount)
    {
        return new TransactionResult(TransactionStatus.Success, amount);
    }

    public static TransactionResult Fail(TransactionStatus status)
    {
        return new TransactionResult(status, 0);
    }

    public override string ToString()
    {
        return IsSuccess ? $"Success ({Amount})" : Status.ToString();
    }
}

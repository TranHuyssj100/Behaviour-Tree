[System.Serializable]
public class ActionNode : BTNode
{
    public CustomerAction action;

    public override NodeState Tick(BTContext ctx)
    {
        return action switch
        {
            CustomerAction.BuyItem => ctx.Agent.BuyItem(ctx.Agent.wantedItem),
            CustomerAction.MoveToShop => ctx.Agent.MoveToTarget(ctx.Agent.targetShop.transform),
            _ => NodeState.Failure
        };
    }
}

public enum CustomerAction
{
    MoveToShop = 0,
    MoveToATM = 1,
    BuyItem = 2,
    UseItem = 3,
    Withdraw = 4
}

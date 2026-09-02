[System.Serializable]
public class WithdrawMoneyAction : BTNode
{
    public override NodeState Tick(BTContext ctx)
    {
        if (ctx?.Agent == null) return NodeState.Failure;
        return ctx.Agent.WithdrawMoney();
    }
}

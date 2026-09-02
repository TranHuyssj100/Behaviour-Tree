[System.Serializable]
public class NotEnoughMoneyCondition : ConditionNode
{
    public override NodeState Tick(BTContext ctx)
    {
        var a = ctx.Agent;
        if (a == null || a.wantedItem == null) return NodeState.Failure;
        return a._money < a.wantedItem.BasePrice
            ? NodeState.Success : NodeState.Failure;
    }
}

[System.Serializable]
public class SelectorNode : CompositeNode
{
    public override NodeState Tick(BTContext ctx)
    {
        if (children == null) return NodeState.Failure;
        foreach (var node in children)
        {
            if (node == null) continue;
            NodeState state = node.Tick(ctx);
            if (state != NodeState.Failure)
                return state;
        }
        return NodeState.Failure;
    }
}

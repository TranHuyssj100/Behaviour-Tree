[System.Serializable]
public class SequenceNode : CompositeNode
{
    public override NodeState Tick(BTContext ctx)
    {
        if (children == null) return NodeState.Success;
        foreach (var child in children)
        {
            if (child == null) continue;
            var state = child.Tick(ctx);
            if (state != NodeState.Success) return state;
        }
        return NodeState.Success;
    }
}

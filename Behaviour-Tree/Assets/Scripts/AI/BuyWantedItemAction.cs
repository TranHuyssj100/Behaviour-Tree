using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuyWantedItemAction : BTNode
{
    public override NodeState Tick(BTContext ctx)
        => ctx.Agent.BuyItem(ctx.Agent.wantedItem);
}
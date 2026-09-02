using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class MoveToShopAction : BTNode
{
    public override NodeState Tick(BTContext ctx)
        => ctx.Agent.MoveToTarget(ctx.Agent.targetShop.transform);
}


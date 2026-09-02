using UnityEngine;

[System.Serializable]
public abstract class BTNode
{
    [SerializeField] string nodeName;

    public string NodeName
    {
        get => nodeName;
        set => nodeName = value;
    }

    public string GetDisplayName()
    {
        return string.IsNullOrWhiteSpace(nodeName) ? GetType().Name : nodeName;
    }

    public abstract NodeState Tick(BTContext ctx);
}

public class BTContext
{
    public CustomerAgent Agent;
}

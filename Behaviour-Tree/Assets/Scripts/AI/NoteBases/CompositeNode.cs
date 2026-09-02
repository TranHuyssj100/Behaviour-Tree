using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class CompositeNode : BTNode
{
    [SerializeReference] public List<BTNode> children = new();
}

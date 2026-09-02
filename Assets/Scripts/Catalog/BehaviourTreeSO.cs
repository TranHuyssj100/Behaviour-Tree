using UnityEngine;

[CreateAssetMenu(fileName = "BehaviourTree", menuName = "Tycoon/BehaviourTree")]
public class BehaviourTreeSO : ScriptableObject
{
    [SerializeReference] public BTNode Root;
}

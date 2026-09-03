using UnityEngine;
using System.Collections.Generic;
using VContainer;
using NavMeshAgent = UnityEngine.AI.NavMeshAgent;

[RequireComponent(typeof(NavMeshAgent))]
public class CustomerAgent : MonoBehaviour
{
    [SerializeField] private List<Transform> _targets;
    [SerializeField] private Transform currentTarget;
    [SerializeField] private bool _isAtTarget = false;
    [SerializeField] private float _currentDistance = 0f;

    [field: SerializeField] public int _money { get; private set; } = 50;
    [SerializeField] private int _pizzaPrice = 10;
    [SerializeField] private bool _isHasPizza = false;
    [SerializeField] private float _stoppingDistance = 0.4f;

    [field: SerializeField] public ShopBase targetShop { get; private set; }
    [field: SerializeField] public IProduct wantedItem { get; private set; }

    [SerializeField] private List<SequenceNode> _sequences;

    private NavMeshAgent _navMeshAgent;
    private Economy _economy;
    [SerializeField] BehaviourTreeSO treeAsset;
    private BTNode _root;
    private BTContext _ctx;

    [Inject]
    public void Construct(Economy economy)
    {
        _economy = economy;
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _ctx = new BTContext { Agent = this };
        if (treeAsset != null)
            _root = treeAsset.Root;
    }

    private void Update()
    {
        if (_root == null) return;
        _root.Tick(_ctx);
    }

    private bool IsAtTarget(Transform target)
    {
        _currentDistance = Vector3.Distance(transform.position, target.position);
        _isAtTarget = _currentDistance <= _stoppingDistance;
        return _isAtTarget;
    }

    public NodeState MoveToTarget(Transform target)
    {
        if (currentTarget != target) currentTarget = target;
        _navMeshAgent.SetDestination(currentTarget.position);
        if (IsAtTarget(target))
        {
            return NodeState.Success;
        }
        return NodeState.Running;
    }

    public NodeState BuyItem(IProduct product)
    {
        int paid = product.ProductPrice;

        if (_economy != null && targetShop != null && product != null)
        {
            TransactionResult result = _economy.SellToCustomer(targetShop, product);
            if (!result.IsSuccess)
                return NodeState.Failure;

            paid = result.Amount;
        }

        _money -= paid;
        _isHasPizza = true;
        return NodeState.Success;
    }

    private NodeState UseItem(ItemDefinitionSO item)
    {
        return NodeState.Success;
    }

    public NodeState WithdrawMoney()
    {
        _money += 20;
        return NodeState.Success;
    }
}

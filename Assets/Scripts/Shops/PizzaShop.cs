using UnityEngine;

public class PizzaShop : ShopBase
{
    [SerializeField] Transform counter;

    public Transform Counter => counter != null ? counter : transform;

    public override void OnSold(ItemDefinitionSO item, int revenue)
    {
        Debug.Log($"{name} sold {item.DisplayName} for {revenue}");
    }
}

using UnityEngine;
using System.Collections.Generic;

public class PizzaShop : ShopBase
{
    [SerializeField] Transform counter;

    [SerializeField] Queue<IProduct> productCompletedQueue = new Queue<IProduct>();

    public Transform Counter => counter != null ? counter : transform;

    public override void OnSold(IProduct product, int revenue)
    {
        Debug.Log($"{name} sold {product.ProductType} for {revenue}");
    }
}

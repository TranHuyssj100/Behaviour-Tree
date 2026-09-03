using UnityEngine;

public abstract class FoodBase : MonoBehaviour, IFood
{
    [SerializeField] protected ProductType productType = ProductType.None;
    [SerializeField] protected FoodType foodType = FoodType.None;
    [SerializeField] protected int productId = 0;
    [SerializeField] protected int itemPrice;

    public ProductType ProductType => productType;
    public FoodType FoodType => foodType;
    public int ProductId => productId;
    public int ProductPrice => itemPrice;
    
    
    public int price
    {
        get => itemPrice;
        set => itemPrice = Mathf.Max(0, value);
    }


    protected void SetId(int value)
    {
        productId = value;
    }

    
    // Implementation of IProduct interface
    public virtual void OnProduced()
    {
        Debug.Log($"Produced {productId}");
    }

    public virtual void OnSold()
    {
        Debug.Log($"Sold {productId}");
    }
}


[System.Serializable]
public enum FoodType
{
    None = 0,
    Pizza = 1,
}   
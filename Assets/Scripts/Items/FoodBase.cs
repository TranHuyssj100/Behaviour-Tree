using UnityEngine;

public abstract class FoodBase : MonoBehaviour, IItem, ITraderable
{
    [SerializeField] string itemId = "food_new";
    [SerializeField] int itemPrice;
    [SerializeField] int itemQuantity = 1;

    public string id => itemId;
    public virtual ItemType type => ItemType.Food;

    public int price
    {
        get => itemPrice;
        set => itemPrice = Mathf.Max(0, value);
    }

    public int quantity
    {
        get => itemQuantity;
        set => itemQuantity = Mathf.Max(0, value);
    }

    protected void SetId(string value)
    {
        itemId = value;
    }

    public virtual void OnUse()
    {
        Debug.Log($"Use {itemId}");
    }

    public virtual void OnBuy()
    {
        Debug.Log($"Buy {itemId}");
    }

    public virtual void OnSell()
    {
        Debug.Log($"Sell {itemId}");
    }
}

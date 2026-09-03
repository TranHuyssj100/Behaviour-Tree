using UnityEngine;

public class Pizza : FoodBase
{
    [SerializeField] private PizzaType pizzaType;

    public PizzaType PizzaType => pizzaType;

    public Pizza(PizzaType pizzaType, int productId, int itemPrice)
    {
        this.pizzaType = pizzaType;
        this.productId = productId;
        this.itemPrice = itemPrice;
    }
}

[System.Serializable]
public enum PizzaType
{
    None = 0,
    Margherita = 1,
    Pepperoni = 2,
    Mushroom = 3,
}

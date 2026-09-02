using UnityEngine;

public class Pizza : FoodBase
{
    [SerializeField] PizzaType pizzaType = PizzaType.None;

    public PizzaType PizzaKind
    {
        get => pizzaType;
        set
        {
            pizzaType = value;
            SyncId();
        }
    }

    void OnEnable()
    {
        SyncId();
    }

    void OnValidate()
    {
        SyncId();
    }

    void SyncId()
    {
        SetId(GetItemId(pizzaType));
    }

    public static string GetItemId(PizzaType type)
    {
        return $"pizza_{type.ToString().ToLowerInvariant()}";
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

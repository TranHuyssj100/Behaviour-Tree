using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "PizzaDataSO", menuName = "Scriptable Objects/PizzaDataSO")]
public class PizzaDataSO : ScriptableObject
{
    public List<PizzaData> ListPizzaData = new List<PizzaData>();

    public PizzaData GetPizzaByType(PizzaType type)
    {
        return ListPizzaData.FirstOrDefault(pizza => pizza.pizzaType == type);
    }
}

[System.Serializable]
public class PizzaData : IItemData
{
    [field: SerializeField] public PizzaType pizzaType { get; set; }
    [field: SerializeField] public int price { get; set; }
    [field: SerializeField] public int quantity { get; set; }

    public string id => Pizza.GetItemId(pizzaType);
}

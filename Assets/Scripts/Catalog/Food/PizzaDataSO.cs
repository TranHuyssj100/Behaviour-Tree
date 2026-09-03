using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[CreateAssetMenu(fileName = "PizzaDataSO", menuName = "Tycoon/PizzaDataSO")]
public class PizzaDataSO : ScriptableObject, IProductDefinition
{
    public ProductType ProductType => ProductType.Food;

    public List<PizzaData> PizzaDataList = new List<PizzaData>();
}



[System.Serializable]
public class PizzaData
{
    public PizzaType PizzaType;
    public int PizzaPrice;
    public float PizzaTimeMaker;
}
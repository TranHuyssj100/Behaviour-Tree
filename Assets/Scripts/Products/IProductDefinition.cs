using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProductDefinition
{
    ProductType ProductType { get; }

}


[System.Serializable]
public enum ProductType
{
    None = 0,
    Food = 1,
    Clothes = 2,
}


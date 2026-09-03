using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFood : IProduct
{
    FoodType FoodType { get; }
}

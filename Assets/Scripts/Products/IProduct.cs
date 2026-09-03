using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProduct : IProductDefinition
{
    int ProductId { get; }
    int ProductPrice { get; }
    void OnProduced();
    void OnSold();
}


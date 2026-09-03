using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFactory
{
    FactoryType FactoryType { get; }
    bool CanProduce(IProduct product);
    void Produce(IProduct product);
}

[System.Serializable]
public enum FactoryType
{
    None,
    Pizza
}
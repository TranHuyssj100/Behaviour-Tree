using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class ShopBase : MonoBehaviour, IProperty, IShop
{
    [SerializeField] ShopDefinitionSO definition;
    [SerializeField] int currentLevel = 1;
    [SerializeField] bool isUnlocked = true;
    [SerializeField] PropertyType propertyType = PropertyType.Building;
    [SerializeField] FactoryType factoryType = FactoryType.None;
    [SerializeField] Queue<IProduct> productCompletedQueue = new();
    [SerializeField] List<IStaff> staffProduce = new();


    public PropertyType PropertyType => propertyType;
    public FactoryType FactoryType => factoryType;
    public Queue<IProduct> ProductCompletedQueue => productCompletedQueue;
    public ShopDefinitionSO Definition => definition;
    public int CurrentLevel => currentLevel;
    public int MaxLevel => definition != null ? definition.MaxLevel : 1;
    public bool IsUnlocked => isUnlocked;
    public event Action<ShopBase> Unlocked;
    public event Action<ShopBase> Upgraded;

    public void Initialize(ShopDefinitionSO shopDefinition, int level = 1, bool unlocked = true)
    {
        definition = shopDefinition;
        currentLevel = Mathf.Clamp(level, 1, MaxLevel);
        isUnlocked = unlocked;
    }

    public virtual void Unlock()
    {
        if (isUnlocked)
            return;

        isUnlocked = true;
        Unlocked?.Invoke(this);
    }

    public virtual void Upgrade()
    {
        if (currentLevel >= MaxLevel)
            return;

        currentLevel++;
        Upgraded?.Invoke(this);
    }

    public abstract void OnSold(IProduct product, int revenue);

    //Factory
    public bool CanProduce(IProduct product)
    {

        if (staffProduce == null || staffProduce.Count == 0)
        {
            Debug.Log($"No staff to produce {product.ProductType} at {name}");
            return false;
        }
        bool canProduce = staffProduce.Count > 0 && staffProduce.Any(staff => staff.CanWork(product as ItemDefinitionSO));
        Debug.Log($"Can produce {product.ProductType} at {name} by {staffProduce.Count} staffs");
        return canProduce;
    }

    public void Produce(IProduct product)
    {
        Debug.Log($"Producing {product.ProductType} at {name}");
    }
}

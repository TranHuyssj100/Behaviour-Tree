using System;
using UnityEngine;

public abstract class ShopBase : MonoBehaviour, IShop, IProperty
{
    [SerializeField] ShopDefinitionSO definition;
    [SerializeField] int currentLevel = 1;
    [SerializeField] bool isUnlocked = true;

    public ShopDefinitionSO Definition => definition;
    public ShopType ShopType => definition != null ? definition.ShopType : ShopType.None;
    public PropertyType PropertyType => PropertyType.Shop;
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

    public virtual bool CanSell(ItemDefinitionSO item)
    {
        return isUnlocked && definition != null && definition.Sells(item);
    }

    public abstract void OnSold(ItemDefinitionSO item, int revenue);
}

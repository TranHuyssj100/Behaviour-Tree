using System;
using UnityEngine;

public class Economy
{
    readonly PlayerProfile profile;
    readonly ShopRegistry registry;
    readonly ShopSpawner spawner;
    readonly ShopRevenue revenue;
    readonly IGameModeRules rules;

    public Economy(
        PlayerProfile profile,
        ShopRegistry registry,
        ShopSpawner spawner,
        ShopRevenue revenue,
        IGameModeRules rules)
    {
        this.profile = profile;
        this.registry = registry;
        this.spawner = spawner;
        this.revenue = revenue;
        this.rules = rules;
    }

    public ShopRegistry Shops => registry;

    public event Action<ShopBase, IProduct, int> ItemSold;
    public event Action<ShopBase> ShopBuilt;
    public event Action<ShopBase> ShopUpgraded;
    public event Action<IProduct> BoostBought;

    public TransactionResult SellToCustomer(ShopBase shop, IProduct product)
    {
        if (shop == null || product == null)
            return TransactionResult.Fail(TransactionStatus.InvalidRequest);

        if (!shop.IsUnlocked)
            return TransactionResult.Fail(TransactionStatus.ShopLocked);

        int price = revenue.GetSalePrice(shop, product);
        profile.AddMoney(price);
        shop.OnSold(product, price);
        ItemSold?.Invoke(shop, product, price);
        return TransactionResult.Ok(price);
    }

    public TransactionResult TryBuildShop(ShopDefinitionSO definition, Vector3 position, Quaternion rotation)
    {
        if (definition == null)
            return TransactionResult.Fail(TransactionStatus.InvalidRequest);

        if (!rules.CanBuildShops)
            return TransactionResult.Fail(TransactionStatus.NotAllowedInMode);

        if (!profile.TrySpend(definition.BuildCost))
            return TransactionResult.Fail(TransactionStatus.NotEnoughMoney);

        ShopBase shop = spawner.Spawn(definition, position, rotation);
        if (shop == null)
        {
            profile.AddMoney(definition.BuildCost);
            return TransactionResult.Fail(TransactionStatus.InvalidRequest);
        }

        ShopBuilt?.Invoke(shop);
        return TransactionResult.Ok(definition.BuildCost);
    }

    public TransactionResult TryUpgrade(ShopBase shop)
    {
        if (shop == null || shop.Definition == null)
            return TransactionResult.Fail(TransactionStatus.InvalidRequest);

        if (!shop.IsUnlocked)
            return TransactionResult.Fail(TransactionStatus.ShopLocked);

        if (!shop.Definition.TryGetUpgradeCost(shop.CurrentLevel, out int cost))
            return TransactionResult.Fail(TransactionStatus.MaxLevelReached);

        if (!profile.TrySpend(cost))
            return TransactionResult.Fail(TransactionStatus.NotEnoughMoney);

        shop.Upgrade();
        ShopUpgraded?.Invoke(shop);
        return TransactionResult.Ok(cost);
    }
}

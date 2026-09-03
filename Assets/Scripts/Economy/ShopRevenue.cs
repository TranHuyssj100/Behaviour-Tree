using UnityEngine;

public class ShopRevenue
{
    readonly IGameModeRules rules;

    public ShopRevenue(IGameModeRules rules)
    {
        this.rules = rules;
    }

    public int GetSalePrice(ShopBase shop, IProduct product)
    {
        if (product == null)
            return 0;

        float levelMultiplier = shop != null && shop.Definition != null
            ? shop.Definition.GetRevenueMultiplier(shop.CurrentLevel)
            : 1f;

        float modeMultiplier = rules != null ? rules.RevenueMultiplier : 1f;
        return Mathf.Max(0, Mathf.RoundToInt(product.ProductPrice * levelMultiplier * modeMultiplier));
    }
}

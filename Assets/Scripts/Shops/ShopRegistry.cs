using System;
using System.Collections.Generic;

public class ShopRegistry
{
    readonly List<ShopBase> shops = new List<ShopBase>();

    public IReadOnlyList<ShopBase> Shops => shops;

    public event Action<ShopBase> ShopRegistered;
    public event Action<ShopBase> ShopUnregistered;

    public void Register(ShopBase shop)
    {
        if (shop == null || shops.Contains(shop))
            return;

        shops.Add(shop);
        ShopRegistered?.Invoke(shop);
    }

    public void Unregister(ShopBase shop)
    {
        if (shop == null || !shops.Remove(shop))
            return;

        ShopUnregistered?.Invoke(shop);
    }

    public ShopBase FindFirst(ShopDefinitionSO definition)
    {
        if (definition == null)
            return null;

        for (int i = 0; i < shops.Count; i++)
        {
            if (shops[i] != null && shops[i].Definition == definition)
                return shops[i];
        }

        return null;
    }

    public int CountOf(ShopDefinitionSO definition)
    {
        if (definition == null)
            return 0;

        int count = 0;
        for (int i = 0; i < shops.Count; i++)
        {
            if (shops[i] != null && shops[i].Definition == definition)
                count++;
        }

        return count;
    }
}

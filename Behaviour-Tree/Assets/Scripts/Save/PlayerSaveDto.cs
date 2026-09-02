using System;
using System.Collections.Generic;

[Serializable]
public class PlayerSaveDto
{
    public const int CurrentVersion = 1;

    public int version = CurrentVersion;
    public int playerId;
    public string playerName = "Player";
    public int level = 1;
    public int money;
    public List<ItemStackDto> items = new List<ItemStackDto>();
    public List<ShopStateDto> shops = new List<ShopStateDto>();

    public static PlayerSaveDto FromGame(PlayerProfile profile, ShopRegistry registry)
    {
        PlayerSaveDto save = new PlayerSaveDto();
        if (profile != null)
        {
            save.playerId = profile.Data.Id;
            save.playerName = profile.Data.PlayerName;
            save.level = profile.Data.Level;
            save.money = profile.Data.Money;

            foreach (ItemStack stack in profile.Items)
                save.items.Add(new ItemStackDto { itemId = stack.ItemId, quantity = stack.Quantity });
        }

        if (registry != null)
        {
            foreach (ShopBase shop in registry.Shops)
            {
                if (shop == null || shop.Definition == null)
                    continue;

                UnityEngine.Vector3 position = shop.transform.position;
                save.shops.Add(new ShopStateDto
                {
                    shopId = shop.Definition.Id,
                    level = shop.CurrentLevel,
                    unlocked = shop.IsUnlocked,
                    posX = position.x,
                    posY = position.y,
                    posZ = position.z
                });
            }
        }

        return save;
    }

    public void ApplyTo(PlayerProfile profile)
    {
        if (profile == null)
            return;

        PlayerData data = new PlayerData
        {
            Id = playerId,
            PlayerName = playerName,
            Level = level,
            Money = money
        };

        List<ItemStack> stacks = new List<ItemStack>();
        if (items != null)
        {
            foreach (ItemStackDto dto in items)
                stacks.Add(new ItemStack { ItemId = dto.itemId, Quantity = dto.quantity });
        }

        profile.Restore(data, stacks);
    }
}

[Serializable]
public class ItemStackDto
{
    public string itemId;
    public int quantity;
}

[Serializable]
public class ShopStateDto
{
    public string shopId;
    public int level;
    public bool unlocked;
    public float posX;
    public float posY;
    public float posZ;
}

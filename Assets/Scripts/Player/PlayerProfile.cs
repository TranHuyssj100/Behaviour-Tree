using System;
using System.Collections.Generic;

public class PlayerProfile
{
    readonly PlayerData data = new PlayerData();
    readonly List<ItemStack> items = new List<ItemStack>();

    public PlayerData Data => data;
    public int Money => data.Money;
    public int Level => data.Level;
    public IReadOnlyList<ItemStack> Items => items;

    public event Action<int> MoneyChanged;
    public event Action<int> LevelChanged;
    public event Action InventoryChanged;

    public void LevelUp()
    {
        data.Level++;
        LevelChanged?.Invoke(data.Level);
    }

    public void AddMoney(int amount)
    {
        if (amount <= 0)
            return;

        data.Money += amount;
        MoneyChanged?.Invoke(data.Money);
    }

    public bool TrySpend(int amount)
    {
        if (amount <= 0 || data.Money < amount)
            return false;

        data.Money -= amount;
        MoneyChanged?.Invoke(data.Money);
        return true;
    }

    public void AddItem(string itemId, int quantity = 1)
    {
        if (string.IsNullOrEmpty(itemId) || quantity <= 0)
            return;

        ItemStack stack = FindStack(itemId);
        if (stack == null)
            items.Add(new ItemStack { ItemId = itemId, Quantity = quantity });
        else
            stack.Quantity += quantity;

        InventoryChanged?.Invoke();
    }

    public bool TryRemoveItem(string itemId, int quantity = 1)
    {
        if (string.IsNullOrEmpty(itemId) || quantity <= 0)
            return false;

        ItemStack stack = FindStack(itemId);
        if (stack == null || stack.Quantity < quantity)
            return false;

        stack.Quantity -= quantity;
        if (stack.Quantity == 0)
            items.Remove(stack);

        InventoryChanged?.Invoke();
        return true;
    }

    public int GetItemQuantity(string itemId)
    {
        ItemStack stack = FindStack(itemId);
        return stack != null ? stack.Quantity : 0;
    }

    public void Restore(PlayerData restored, IEnumerable<ItemStack> restoredItems)
    {
        if (restored != null)
        {
            data.Id = restored.Id;
            data.PlayerName = restored.PlayerName;
            data.Level = restored.Level;
            data.Money = restored.Money;
        }

        items.Clear();
        if (restoredItems != null)
            items.AddRange(restoredItems);

        MoneyChanged?.Invoke(data.Money);
        LevelChanged?.Invoke(data.Level);
        InventoryChanged?.Invoke();
    }

    ItemStack FindStack(string itemId)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ItemId == itemId)
                return items[i];
        }

        return null;
    }
}

[Serializable]
public class ItemStack
{
    public string ItemId;
    public int Quantity;
}

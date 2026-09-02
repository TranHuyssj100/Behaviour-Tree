using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class Player : MonoBehaviour
{
    PlayerProfile profile;

    public PlayerProfile Profile => profile ??= new PlayerProfile();
    public PlayerData Data => Profile.Data;
    public int Money => Profile.Money;
    public IReadOnlyList<ItemStack> Items => Profile.Items;

    [Inject]
    public void Construct(PlayerProfile injected)
    {
        profile = injected;
    }

    public void LevelUp() => Profile.LevelUp();

    public void AddMoney(int amount) => Profile.AddMoney(amount);

    public bool TrySpend(int amount) => Profile.TrySpend(amount);

    public void AddItem(ItemDefinitionSO item, int quantity = 1)
    {
        if (item != null)
            Profile.AddItem(item.Id, quantity);
    }

    public void AddItem(IItem item, int quantity = 1)
    {
        if (item != null)
            Profile.AddItem(item.id, quantity);
    }

    public bool TryRemoveItem(ItemDefinitionSO item, int quantity = 1)
    {
        return item != null && Profile.TryRemoveItem(item.Id, quantity);
    }

    public int GetItemQuantity(string itemId) => Profile.GetItemQuantity(itemId);
}

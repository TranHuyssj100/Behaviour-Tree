using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopDefinition", menuName = "Tycoon/Shop Definition")]
public class ShopDefinitionSO : ScriptableObject
{
    [SerializeField] string id = "shop_new";
    [SerializeField] string displayName = "New Shop";
    [SerializeField] ShopType shopType = ShopType.None;
    [SerializeField] int buildCost = 500;
    [SerializeField] ShopBase prefab;
    [SerializeField] List<ItemDefinitionSO> menu = new List<ItemDefinitionSO>();
    [SerializeField] UpgradeTableSO upgradeTable;

    public string Id => id;
    public string DisplayName => displayName;
    public ShopType ShopType => shopType;
    public int BuildCost => buildCost;
    public ShopBase Prefab => prefab;
    public IReadOnlyList<ItemDefinitionSO> Menu => menu;
    public int MaxLevel => upgradeTable != null ? upgradeTable.MaxLevel : 1;

    public bool Sells(ItemDefinitionSO item)
    {
        return item != null && menu.Contains(item);
    }

    public float GetRevenueMultiplier(int level)
    {
        return upgradeTable != null ? upgradeTable.GetRevenueMultiplier(level) : 1f;
    }

    public bool TryGetUpgradeCost(int currentLevel, out int cost)
    {
        if (upgradeTable == null)
        {
            cost = 0;
            return false;
        }

        return upgradeTable.TryGetUpgradeCost(currentLevel, out cost);
    }

    void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(id))
            id = name.ToLowerInvariant().Replace(' ', '_');

        buildCost = Mathf.Max(0, buildCost);
    }
}

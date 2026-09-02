using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeTable", menuName = "Tycoon/Upgrade Table")]
public class UpgradeTableSO : ScriptableObject
{
    [SerializeField] List<UpgradeLevel> levels = new List<UpgradeLevel>();

    public int MaxLevel => Mathf.Max(1, levels.Count);

    public float GetRevenueMultiplier(int level)
    {
        if (levels.Count == 0)
            return 1f;

        int index = Mathf.Clamp(level - 1, 0, levels.Count - 1);
        return levels[index].revenueMultiplier;
    }

    public bool TryGetUpgradeCost(int currentLevel, out int cost)
    {
        cost = 0;
        if (currentLevel < 1 || currentLevel >= MaxLevel)
            return false;

        cost = levels[currentLevel - 1].upgradeCost;
        return true;
    }
}

[System.Serializable]
public class UpgradeLevel
{
    public float revenueMultiplier = 1f;
    public int upgradeCost = 100;
}

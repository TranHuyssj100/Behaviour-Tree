using UnityEngine;

public class BoostItem
{
    public BoostItem(ItemDefinitionSO definition)
    {
        Definition = definition;
        RemainingSeconds = definition != null ? definition.DurationSeconds : 0f;
    }

    public ItemDefinitionSO Definition { get; }
    public float RemainingSeconds { get; private set; }

    public bool IsActive => RemainingSeconds > 0f;

    public float RevenueMultiplier => IsActive && Definition != null
        ? Definition.RevenueMultiplier
        : 1f;

    public void Tick(float deltaTime)
    {
        if (RemainingSeconds > 0f)
            RemainingSeconds = Mathf.Max(0f, RemainingSeconds - deltaTime);
    }
}

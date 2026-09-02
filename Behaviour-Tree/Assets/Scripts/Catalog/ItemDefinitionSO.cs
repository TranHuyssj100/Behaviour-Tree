using UnityEngine;

[CreateAssetMenu(fileName = "ItemDefinition", menuName = "Tycoon/Item Definition")]
public class ItemDefinitionSO : ScriptableObject
{
    [SerializeField] string id = "item_new";
    [SerializeField] string displayName = "New Item";
    [SerializeField] ItemType category = ItemType.Food;
    [SerializeField] int basePrice = 10;

    [Header("Boost")]
    [SerializeField] bool isBoost;
    [SerializeField] float revenueMultiplier = 1f;
    [SerializeField] float durationSeconds = 30f;

    public string Id => id;
    public string DisplayName => displayName;
    public ItemType Category => category;
    public int BasePrice => basePrice;
    public bool IsBoost => isBoost;
    public float RevenueMultiplier => revenueMultiplier;
    public float DurationSeconds => durationSeconds;

    void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(id))
            id = name.ToLowerInvariant().Replace(' ', '_');

        basePrice = Mathf.Max(0, basePrice);
        revenueMultiplier = Mathf.Max(0f, revenueMultiplier);
        durationSeconds = Mathf.Max(0f, durationSeconds);
    }
}

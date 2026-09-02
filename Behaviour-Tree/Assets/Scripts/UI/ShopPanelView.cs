using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ShopPanelView : MonoBehaviour
{
    [SerializeField] ShopDefinitionSO definition;
    [SerializeField] Transform buildPoint;
    [SerializeField] TMP_Text titleLabel;
    [SerializeField] TMP_Text costLabel;
    [SerializeField] Button buildButton;
    [SerializeField] Button upgradeButton;

    Economy economy;

    [Inject]
    public void Construct(Economy injected)
    {
        economy = injected;
    }

    void Awake()
    {
        if (buildButton != null)
            buildButton.onClick.AddListener(OnBuildClicked);

        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(OnUpgradeClicked);
    }

    void OnEnable()
    {
        Render();
    }

    void OnDestroy()
    {
        if (buildButton != null)
            buildButton.onClick.RemoveListener(OnBuildClicked);

        if (upgradeButton != null)
            upgradeButton.onClick.RemoveListener(OnUpgradeClicked);
    }

    void OnBuildClicked()
    {
        if (economy == null || definition == null)
            return;

        Transform point = buildPoint != null ? buildPoint : transform;
        economy.TryBuildShop(definition, point.position, point.rotation);
        Render();
    }

    void OnUpgradeClicked()
    {
        if (economy == null || definition == null)
            return;

        ShopBase shop = economy.Shops.FindFirst(definition);
        if (shop == null)
            return;

        economy.TryUpgrade(shop);
        Render();
    }

    void Render()
    {
        if (definition == null)
            return;

        if (titleLabel != null)
            titleLabel.text = definition.DisplayName;

        ShopBase shop = economy != null ? economy.Shops.FindFirst(definition) : null;

        if (buildButton != null)
            buildButton.gameObject.SetActive(shop == null);

        if (upgradeButton != null)
            upgradeButton.gameObject.SetActive(shop != null);

        if (costLabel == null)
            return;

        if (shop == null)
        {
            costLabel.text = $"{definition.BuildCost:N0} $";
            return;
        }

        costLabel.text = definition.TryGetUpgradeCost(shop.CurrentLevel, out int cost)
            ? $"{cost:N0} $"
            : "MAX";
    }
}

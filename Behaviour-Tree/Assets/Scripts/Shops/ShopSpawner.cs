using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ShopSpawner
{
    readonly IObjectResolver resolver;
    readonly ShopRegistry registry;

    public ShopSpawner(IObjectResolver resolver, ShopRegistry registry)
    {
        this.resolver = resolver;
        this.registry = registry;
    }

    public ShopBase Spawn(ShopDefinitionSO definition, Vector3 position, Quaternion rotation, int level = 1)
    {
        if (definition == null || definition.Prefab == null)
            return null;

        ShopBase shop = resolver.Instantiate(definition.Prefab, position, rotation);
        shop.Initialize(definition, level);
        registry.Register(shop);
        return shop;
    }

    public void Despawn(ShopBase shop)
    {
        if (shop == null)
            return;

        registry.Unregister(shop);
        Object.Destroy(shop.gameObject);
    }
}

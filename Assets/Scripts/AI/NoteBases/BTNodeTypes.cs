using System;
using System.Linq;

public static class BTNodeTypes
{
    public static Type[] All()
    {
        var baseType = typeof(BTNode);
        return baseType.Assembly.GetTypes()
            .Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                !t.IsGenericType &&
                baseType.IsAssignableFrom(t) &&
                t.GetConstructor(Type.EmptyTypes) != null)
            .OrderBy(t => CategoryOrder(t))
            .ThenBy(t => t.Name)
            .ToArray();
    }

    public static string Category(Type t)
    {
        if (typeof(CompositeNode).IsAssignableFrom(t)) return "Composite";
        if (typeof(ConditionNode).IsAssignableFrom(t)) return "Condition";
        return "Action";
    }

    static int CategoryOrder(Type t)
    {
        if (typeof(CompositeNode).IsAssignableFrom(t)) return 0;
        if (typeof(ConditionNode).IsAssignableFrom(t)) return 1;
        return 2;
    }
}

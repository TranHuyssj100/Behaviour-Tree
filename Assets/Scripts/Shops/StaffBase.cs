using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StaffBase : MonoBehaviour, IStaff
{
    [SerializeField] StaffType staffType;
    [SerializeField] int currentLevel = 1;
    [SerializeField] int maxLevel = 10;
    [SerializeField] bool isUnlocked = false;

    public StaffType StaffType => staffType;
    public int CurrentLevel => currentLevel;
    public int MaxLevel => maxLevel;
    public bool IsUnlocked => isUnlocked;

    public void Unlock()
    {
        isUnlocked = true;
    }

    public void Upgrade()
    {
        currentLevel++;
    }

    public bool CanWork(ItemDefinitionSO item)
    {
        return isUnlocked && currentLevel < maxLevel;
    }

    public virtual void Work(ItemDefinitionSO item)
    {
        Debug.Log($"Working {item.name} at {name}");
    }
}

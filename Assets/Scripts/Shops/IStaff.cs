using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStaff 
{
    StaffType StaffType { get; }
    int CurrentLevel { get; }
    int MaxLevel { get; }
    bool IsUnlocked { get; }
    void Unlock();  
    void Upgrade();
    bool CanWork(ItemDefinitionSO item);
    void Work(ItemDefinitionSO item);
}

[System.Serializable]
public enum StaffType
{
    None,
    Producer,
    Trader,
    Other,
}
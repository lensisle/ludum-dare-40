using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Weight,
    Strenght,
    Cold,
    ColdResistance,
    Speed
}

[System.Serializable]
public class Stat
{
    public StatType Type;
    public int Value;
}

public class Stats
{
    private Dictionary<StatType, Stat> _stats;

    public Stats(List<Stat> stats)
    {
        _stats = new Dictionary<StatType, Stat>();
        foreach (Stat stat in stats)
        {
            _stats.Add(stat.Type, stat);
        }
    }

    public Stat GetStat(StatType type)
    {
        return _stats[type];
    }

    public void SetStatValue(StatType type, int quantity, bool addToCurrentValue = false)
    {
        _stats[type].Value = addToCurrentValue ? _stats[type].Value + quantity : quantity;
    }
}

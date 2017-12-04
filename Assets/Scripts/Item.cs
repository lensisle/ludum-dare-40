using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemRatity
{
    Common,
    Rare,
    Legendary
}


[CreateAssetMenu(fileName = "ItemData", menuName = "Data/Item", order = 2)]
public class Item : ScriptableObject
{
    public string Name;
    public string DisplayName;
    public List<Stat> Stats;
    public Sprite Icon;
    public ItemRatity Rarity;

    public Item Clone()
    {
        return (Item) MemberwiseClone();
    }

    public int GetStatValue(StatType type)
    {
        Stat stat = Stats.Find(x => x.Type.Equals(type));
        if (stat != null)
        {
            return stat.Value;
        }
        return 0;
    }
}

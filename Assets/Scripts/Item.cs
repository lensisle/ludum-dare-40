using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Data/Item", order = 2)]
public class Item : ScriptableObject
{
    public string Name;
    public string DisplayName;
    public List<Stat> Stats;
    public Sprite Icon;

    public Item Clone()
    {
        return (Item) MemberwiseClone();
    }
}

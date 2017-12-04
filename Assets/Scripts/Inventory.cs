using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<string, Item> _items;

    public Inventory()
    {
        _items = new Dictionary<string, Item>();
    }

    public bool AddItem(Item item)
    {
        if (_items.ContainsKey(item.Name))
        {
            return false;
        }
        
        _items.Add(item.Name, item);
        return true;
    }

    public void RemoveItem(string name)
    {
        _items.Remove(name);
    }

    public Item GetItem(string name)
    {
        return _items[name];
    }
}

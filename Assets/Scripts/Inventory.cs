using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<string, Item> _items;
    private int _addedItems;

    public Inventory()
    {
        _items = new Dictionary<string, Item>();
    }

    public void AddItem(Item item)
    {
        _addedItems += 1;
        _items.Add(item.Name, item);
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

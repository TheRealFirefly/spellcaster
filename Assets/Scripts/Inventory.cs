using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private void Awake()
    {
        instance = this;
    }

    public List<InventoryItem> items = new List<InventoryItem>();

    public void Add(ItemData item)
    {
        InventoryItem existing = items.Find(i => i.itemData == item);

        if (existing != null)
        {
            existing.count++; 
            Debug.Log("Item gestapelt: " + item.itemName + " (" + existing.count + ")");
        }
        else
        {
            items.Add(new InventoryItem(item, 1)); 
            Debug.Log("Neues Item hinzugefügt: " + item.itemName);
        }
    }
}

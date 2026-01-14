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
        // Prüfen, ob Item schon existiert
        InventoryItem existing = items.Find(i => i.itemData == item);

        if (existing != null)
        {
            existing.count++; // Stack erhöhen
            Debug.Log("Item gestapelt: " + item.itemName + " (" + existing.count + ")");
        }
        else
        {
            items.Add(new InventoryItem(item, 1)); // Neues Item hinzufügen
            Debug.Log("Neues Item hinzugefügt: " + item.itemName);
        }
    }
}

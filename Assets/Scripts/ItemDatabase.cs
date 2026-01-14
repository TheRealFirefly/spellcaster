using NUnit.Framework;
using NUnit.Framework.Interfaces;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    public ItemData[] allItems;

    private void Awake()
    {
        instance = this;
    }

    public ItemData GetRandomItem(ItemRarity rarity)
    {
        var filtered = System.Array.FindAll(allItems, i => i.rarity == rarity);

        if (filtered.Length == 0)
        {
            Debug.LogWarning("Keine Items für Rarity: " + rarity);
            return null;
        }

        return filtered[Random.Range(0, filtered.Length)];
    }
}

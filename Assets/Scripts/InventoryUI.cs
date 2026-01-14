using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    public Transform container;
    public GameObject entryPrefab;
    public Sprite[] rarityBorder;

    private void Awake()
    {
        instance = this;
    }

    public void Refresh()
    {
        // Alte Einträge löschen
        foreach (Transform t in container)
            Destroy(t.gameObject);

        // Neue Einträge erstellen
        foreach (var invItem in Inventory.instance.items)
        {
            GameObject go = Instantiate(entryPrefab, container);

            ItemData item = invItem.itemData;

            // Name
            TMP_Text nameText = go.transform.Find("Name").GetComponent<TMP_Text>();
            nameText.text = item.itemName;

            // Icon
            go.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;

            // Rarity Border
            go.transform.Find("RarityBorder").GetComponent<Image>().sprite = rarityBorder[(int)item.rarity];

            // Tooltip
            go.GetComponentInChildren<TooltipTrigger>().tooltipText = item.description;

            // Count anzeigen, nur wenn >1
            TMP_Text countText = go.transform.Find("Amount").GetComponent<TMP_Text>();
            if (invItem.count > 1)
            {
                countText.text = invItem.count.ToString();
                countText.enabled = true;
            }
            else
            {
                countText.enabled = false;
            }
        }
    }
}

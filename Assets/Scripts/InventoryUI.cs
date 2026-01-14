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
        foreach (Transform t in container)
            Destroy(t.gameObject);

        foreach (var invItem in Inventory.instance.items)
        {
            GameObject go = Instantiate(entryPrefab, container);

            ItemData item = invItem.itemData;

            TMP_Text nameText = go.transform.Find("Name").GetComponent<TMP_Text>();
            nameText.text = item.itemName;

            go.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;

            go.transform.Find("RarityBorder").GetComponent<Image>().sprite = rarityBorder[(int)item.rarity];

            go.GetComponentInChildren<TooltipTrigger>().tooltipText = item.description;

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

using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemSlot : MonoBehaviour
{
    public Image icon;
    public Image border;
    public TMP_Text nameText;
    public TMP_Text priceText;
    public Button buyButton;
    public TooltipTrigger trigger;
    public GameObject slot;

    public Sprite[] rarityBorder;
    private ItemData currentItem;

    public void SetItem(ItemData item)
    {
        currentItem = item;

        if (item == null)
        {
            icon.enabled = false;
            border.enabled = false;
            trigger.tooltipText = "";
            nameText.text = "";
            priceText.text = "";
            buyButton.interactable = false;
            return;
        }

        icon.enabled = true;
        icon.sprite = item.icon;
        trigger.tooltipText = item.description;
        nameText.text = item.itemName;
        border.enabled=true;
        if(item.rarity == ItemRarity.Common)
        {
            border.sprite = rarityBorder[0];
        }
        else if(item.rarity == ItemRarity.Rare)
        {
             border.sprite = rarityBorder[1];
        }
        else if(item.rarity == ItemRarity.Epic)
        {
            border.sprite = rarityBorder[2];
        }
        else if(item.rarity == ItemRarity.Legendary)
        {
            border.sprite = rarityBorder[3];
        }
        else
        {
            border.sprite = rarityBorder[4];
        }
            int price = ShopManager.instance.GetPrice(item.rarity);
        priceText.text = price + " G";

        buyButton.interactable = true;
    }

    public void OnBuy()
    {
        int price = ShopManager.instance.GetPrice(currentItem.rarity);

        if (PlayerStats.instance.money < price)
            return;

        PlayerStats.instance.money -= price;
        if (currentItem.rarity != ItemRarity.Consumable)
        {
            Inventory.instance.Add(currentItem);
            PlayerStats.instance.ApplyItem(currentItem);
        }
        else
        {
            PlayerStats.instance.consumable = currentItem;
        }

        SetItem(null);
        InventoryUI.instance.Refresh();

        ShopManager.instance.UpdateMoney();

        slot.SetActive(false);
    }
}

[System.Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int count;

    public InventoryItem(ItemData item, int amount)
    {
        itemData = item;
        count = amount;
    }
}

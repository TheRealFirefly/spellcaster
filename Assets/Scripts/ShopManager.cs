using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [Header("UI")]
    public GameObject shopUI;
    public ShopItemSlot prefab;
    public Transform ItemArea;
    public ShopItemSlot[] itemSlots;
    public InventoryUI inventoryUI;
    public TMP_Text money;
    public TMP_Text interest;
    public TMP_Text chances;

    private int rerollCost;
    private int currentLevel;
    public GameObject inGameUI;
    public TMP_Text rerollButton;

    public PlayerController playerController;

    private void Awake()
    {
        instance = this;
    }
    public void UpdateMoney()
    {
        money.text = PlayerStats.instance.money + "g";
        int currentMoney = (int)PlayerStats.instance.money;
        int interestValue = Mathf.Min(currentMoney / 10, 5); 
        interest.text = "Interest: " + interestValue + "g";
    }
    public void OpenShop(int level)
    {
        playerController.paused = true;
        Time.timeScale = 0f;
        inGameUI.SetActive(false);
        shopUI.SetActive(true);

        currentLevel = level;
        CalculateChances(currentLevel);
        UpdateChancesUI();

        GenerateShopItems();     
        inventoryUI.Refresh();

        rerollCost = 2;
        rerollButton.text = $"Reroll ({rerollCost}g)";

        
        PlayerStats.instance.interest();
        PlayerStats.instance.AddMoney(5 + WaveManager.instance.currentWave * 2);
        UpdateMoney();
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        Time.timeScale = 1f;

        WaveManager.instance.StartNextWave();
        playerController.paused = false;
        inGameUI.SetActive(true);
    }

    public void GenerateShopItems()
    {
        foreach (Transform child in ItemArea)
            Destroy(child.gameObject);
        itemSlots = new ShopItemSlot[5];
        for (int i = 0; i < itemSlots.Length -1; i++)
        {
            ItemRarity rarity = GetRandomRarity();
            ItemData item = ItemDatabase.instance.GetRandomItem(rarity);

            ShopItemSlot newSlot = Instantiate(prefab, ItemArea);

            newSlot.SetItem(item);
            newSlot.buyButton.onClick.AddListener(() => newSlot.OnBuy());

            itemSlots[i] = newSlot;
        }
        ItemData consumable = ItemDatabase.instance.GetRandomItem(ItemRarity.Consumable);
        ShopItemSlot consumableSlot = Instantiate(prefab, ItemArea);
        consumableSlot.SetItem(consumable);
        consumableSlot.buyButton.onClick.AddListener(() => consumableSlot.OnBuy());
        itemSlots[4] = consumableSlot;
    }


    public void Reroll()
    {
        if (PlayerStats.instance.money < rerollCost)
            return;

        PlayerStats.instance.money -= rerollCost;
        GenerateShopItems();
        rerollCost = rerollCost * 2;
        rerollButton.text = $"Reroll ({rerollCost}g)";
        inventoryUI.Refresh();
        UpdateMoney();
    }

    public int GetPrice(ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemRarity.Common: return 3;
            case ItemRarity.Rare: return 5;
            case ItemRarity.Epic: return 7;
            case ItemRarity.Legendary: return 10;
            case ItemRarity.Consumable: return 5;
            default: return 3;
        }
    }

    [System.Serializable]
    public struct RarityCurve
    {
        public int startPercent;
        public int endPercent;
        public int startLevel;
        public int endLevel;

        public int GetPercent(int level)
        {
            if (level < startLevel) return startPercent;
            if (level > endLevel) return endPercent;

            float t = Mathf.InverseLerp(startLevel, endLevel, level);
            return Mathf.RoundToInt(Mathf.Lerp(startPercent, endPercent, t));
        }
    }

    public RarityCurve commonCurve = new RarityCurve { startPercent = 65, endPercent = 40, startLevel = 1, endLevel = 10 };
    public RarityCurve rareCurve = new RarityCurve { startPercent = 25, endPercent = 30, startLevel = 1, endLevel = 10 };
    public RarityCurve epicCurve = new RarityCurve { startPercent = 7, endPercent = 20, startLevel = 1, endLevel = 10 };
    public RarityCurve legendaryCurve = new RarityCurve { startPercent = 3, endPercent = 10, startLevel = 1, endLevel = 10 };

    private int cPercent;
    private int rPercent;
    private int ePercent;
    private int lPercent;

    public void CalculateChances(int level)
    {
        float c = commonCurve.GetPercent(level);
        float r = rareCurve.GetPercent(level);
        float e = epicCurve.GetPercent(level);
        float l = legendaryCurve.GetPercent(level);

        if (r >= c) r = c - 5;

        float total = c + r + e + l;

        cPercent = Mathf.RoundToInt(c / total * 100f);
        rPercent = Mathf.RoundToInt(r / total * 100f);
        ePercent = Mathf.RoundToInt(e / total * 100f);
        lPercent = 100 - (cPercent + rPercent + ePercent);
    }

    public void UpdateChancesUI()
    {
        chances.text =
            $"<color=#808080>Common: {cPercent}%</color>  |  " +
            $"<color=#03a9f4>Rare: {rPercent}%</color>  |  " +
            $"<color=#7b1fa2>Epic: {ePercent}%</color>  |  " +
            $"<color=#ff6f00>Legendary: {lPercent}%</color>";
    }

    public ItemRarity GetRandomRarity()
    {
        int roll = Random.Range(0, 100);

        if (roll < cPercent) return ItemRarity.Common;
        if (roll < cPercent + rPercent) return ItemRarity.Rare;
        if (roll < cPercent + rPercent + ePercent) return ItemRarity.Epic;
        return ItemRarity.Legendary;
    }

}

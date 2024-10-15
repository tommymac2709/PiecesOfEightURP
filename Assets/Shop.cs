using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevTV.Saving;
using Newtonsoft.Json.Linq;

public class Shop : MonoBehaviour, IInteractable, IJsonSaveable
{
    
    [SerializeField] string shopName;
    [Range(0f, 100f)]
    [SerializeField] float sellingPercentage = 80f;

    [SerializeField]
    StockItemConfig[] stockConfig;

    [System.Serializable]
    class StockItemConfig
    {
        public InventoryItem item;
        public int initialStock;
        [Range(-50, 100)]
        public float buyingDiscountPercentage;
        [Range(-100, 100)]
        public float itemSellPercentage = 10f;
        public int levelToUnlock = 0;

    }

    Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();
    Dictionary<InventoryItem, int> stock = new Dictionary<InventoryItem, int>();

    Shopper currentShopper = null;

    bool isBuyingMode = true;

    ItemCategory filter = ItemCategory.None;

    public event Action onChange;

    private void Awake()
    {
        foreach (StockItemConfig config in stockConfig)
        {
            stock[config.item] = config.initialStock;
        }
    }

    public void SetShopper(Shopper shopper)
    {
        currentShopper = shopper;
    }

    public IEnumerable<ShopItem> GetFilteredItems() 
    {
        foreach (ShopItem shopItem in GetAllItems())
        {
            InventoryItem inventoryItem = shopItem.GetInventoryItem();
            if (filter == ItemCategory.None || inventoryItem.GetCategory() == filter)
            {
                yield return shopItem;
            }
        }
    }

    public IEnumerable<ShopItem> GetAllItems()
    {
        int level = GetShopperLevel();

        foreach (StockItemConfig config in stockConfig)
        {
            if (config.levelToUnlock > level) { continue; }
            
            float price = GetPrice(config);

            int quantityInTransaction = 0;
            transaction.TryGetValue(config.item, out quantityInTransaction);
            int availability = GetAvailability(config.item);

            yield return new ShopItem(config.item, availability, price, quantityInTransaction);
        }
    }

    private int GetAvailability(InventoryItem item)
    {
        if (isBuyingMode)
        {
            return stock[item];
        }

        return CountItemsInInventory(item);
        
    }

    private int CountItemsInInventory(InventoryItem item)
    {
        Inventory inventory = currentShopper.GetComponent<Inventory>();
        if (inventory == null) return 0;

        int total = 0;
        for (int i = 0; i < inventory.GetSize(); i++)
        {
            if (inventory.GetItemInSlot(i) == item)
            {
                total += inventory.GetNumberInSlot(i);
            }
        }
        return total;
    }

    private float GetPrice(StockItemConfig config)
    {
        if (isBuyingMode)
        {
            return config.item.GetPrice() * (1 - config.buyingDiscountPercentage / 100);
        }

        return Mathf.Max(config.item.GetPrice() * ((sellingPercentage / 100f) - (config.itemSellPercentage / 100f)), 0);

    }

    public void SelectFilter(ItemCategory category) 
    {
        filter = category;
        print(category);
        if (onChange != null)
        {
            onChange();
        }
    }
    public ItemCategory GetItemFilter() 
    { 
        return filter; 
    }
    public void SelectMode(bool isBuying) 
    { 
        isBuyingMode = isBuying;
        if (onChange != null)
        {
            onChange();
        }
    }
    public bool IsBuyingMode() 
    { 
        return isBuyingMode; 
    }
    public bool CanTransact() 
    {
        if (IsTransactionEmpty()) return false;
        if (!HasSufficientFunds()) return false;
        if (!HasInventorySpace()) return false;

        return true; 
    }

    public bool HasInventorySpace()
    {
        if (!isBuyingMode) return true;

        Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
        if (shopperInventory == null) return false;
        List<InventoryItem> flatItems = new List<InventoryItem>();
        foreach (ShopItem shopItem in GetAllItems())
        {
            

            InventoryItem item = shopItem.GetInventoryItem();
            int quantity = shopItem.GetQuantityInTransaction();

            for (int i = 0; i < quantity; i++)
            {
                flatItems.Add(item);
            }
        }

        return shopperInventory.HasSpaceFor(flatItems);
    }

    public bool HasSufficientFunds()
    {
        if (!isBuyingMode) return true;

        Purse purse = currentShopper.GetComponent<Purse>();
        if (purse == null) return false;

        return purse.GetBalance() >= TransactionTotal();
    }

    public bool IsTransactionEmpty()
    {
        return transaction.Count == 0;
    }

    public float TransactionTotal() 
    {
        float total = 0f;

        foreach (ShopItem item in GetAllItems())
        {
            total += item.GetPrice() * item.GetQuantityInTransaction();
        }
        return total;
    }


    public void AddToTransaction(InventoryItem item, int quantity) 
    {
        if (!transaction.ContainsKey(item))
        {
            transaction[item] = 0;
        }

        int availability = GetAvailability(item);

        if (transaction[item] + quantity > availability) 
        {
            transaction[item] = availability;
        
        }
        else
        {
            transaction[item] += quantity;
        }

        

        if (transaction[item] <= 0)
        {
            transaction.Remove(item);
        }

        if (onChange != null)
        {
            onChange();
        }
    }
    public void ConfirmTransaction() 
    { 
        Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
        Purse shopperPurse = currentShopper.GetComponent<Purse>();
        if (shopperInventory == null || shopperPurse == null) return;

        

        foreach (ShopItem shopItem in GetAllItems())
        {
            InventoryItem item = shopItem.GetInventoryItem();
            int quantity = shopItem.GetQuantityInTransaction();
            float price = shopItem.GetPrice();
            for (int i = 0; i < quantity; i++)
            {
                if (isBuyingMode)
                {
                    BuyItem(shopperInventory, shopperPurse, item, price);
                }
                else
                {
                    SellItem(shopperInventory, shopperPurse, item, price);
                }

            }

        }

        if (onChange != null)
        {
            onChange();
        }


    }

    private void SellItem(Inventory shopperInventory, Purse shopperPurse, InventoryItem item, float price)
    {
        int slot = FindFirstItemSlot( shopperInventory, item);
        if (slot == -1) return;
        
        AddToTransaction(item, -1);
        shopperInventory.RemoveFromSlot(slot, 1);
        stock[item]++;
        shopperPurse.UpdateBalance(price);
        
    }

    private int FindFirstItemSlot(Inventory shopperInventory, InventoryItem item)
    {
        for (int i = 0; i < shopperInventory.GetSize(); i++)
        {
            if (shopperInventory.GetItemInSlot(i) == item)
            {
                return i;
            }
        }
        return -1;
    }

    private void BuyItem(Inventory shopperInventory, Purse shopperPurse, InventoryItem item, float price)
    {
        if (shopperPurse.GetBalance() < price) return;

        bool success = shopperInventory.AddToFirstEmptySlot(item, 1);

        if (success)
        {
            AddToTransaction(item, -1);
            stock[item]--;
            shopperPurse.UpdateBalance(-price);
        }
    }

    public void Interact(Transform interactorTransform)
    {
        var shopper = GameObject.FindWithTag("Player").GetComponent<Shopper>();
        shopper.SetActiveShop(this);
    }

    public string GetShopName()
    {
        return shopName;
    }

    private int GetShopperLevel()
    {
        BaseStats stats = currentShopper.GetComponent<BaseStats>();
        if (stats == null) return 0;

        return stats.GetLevel();

    }

    public string GetInteractText()
    {
        return "Shop: " + shopName;
    }

    public Transform GetTransform()
    {
        return this.transform;
    }

    public JToken CaptureAsJToken()
    {
        JObject state = new JObject();
        IDictionary<string, JToken> stateDict = state;
        foreach (KeyValuePair<InventoryItem, int> pair in stock)
        {
            stateDict[pair.Key.GetItemID()] = JToken.FromObject(pair.Value);
        }
        return state;
    }

    public void RestoreFromJToken(JToken state)
    {
        if (state is JObject stateObject)
        {
            IDictionary<string, JToken> stateDict = stateObject;
            stock.Clear();
            foreach (KeyValuePair<string, JToken> pair in stateDict)
            {
                InventoryItem item = InventoryItem.GetFromID(pair.Key);
                if (item)
                {
                    stock[item] = pair.Value.ToObject<int>();
                }
            }
        }
    }

}

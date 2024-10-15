using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.ShaderGraph.Internal;

public class Shop : MonoBehaviour, IInteractable
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

    }

    Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();
    Dictionary<InventoryItem, int> stock = new Dictionary<InventoryItem, int>();

    Shopper currentShopper = null;

    bool isBuyingMode = true;

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
        return GetAllItems();
    }

    public IEnumerable<ShopItem> GetAllItems()
    {
        foreach (StockItemConfig config in stockConfig)
        {
            float price = GetPrice(config);

            int quantityInTransaction = 0;
            transaction.TryGetValue(config.item, out quantityInTransaction);

            int currentStock = stock[config.item];

            yield return new ShopItem(config.item, currentStock, price, quantityInTransaction);
        }
    }

    private float GetPrice(StockItemConfig config)
    {
        if (isBuyingMode)
        {
            return config.item.GetPrice() * (1 - config.buyingDiscountPercentage / 100);
        }

        return Mathf.Max(config.item.GetPrice() * ((sellingPercentage / 100f) - (config.itemSellPercentage / 100f)), 0);

    }

    public void SelectFilter(ItemCategory category) { }
    public ItemCategory GetItemFilter() { return ItemCategory.None; }
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

        if (transaction[item] + quantity > stock[item]) 
        {
            transaction[item] = stock[item];
        
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
                if (shopperPurse.GetBalance() < price) { break; }

                bool success = shopperInventory.AddToFirstEmptySlot(item, 1);

                if (success)
                {
                    AddToTransaction(item, -1);
                    stock[item]--;
                    shopperPurse.UpdateBalance(-price);
                }
            }

            

        }

        if (onChange != null)
        {
            onChange();
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

    public string GetInteractText()
    {
        return "Shop: " + shopName;
    }

    public Transform GetTransform()
    {
        return this.transform;
    }
}

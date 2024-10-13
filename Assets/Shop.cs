using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shop : MonoBehaviour, IInteractable
{
    
    [SerializeField] string shopName;

    [SerializeField]
    StockItemConfig[] stockConfig;

    [System.Serializable]
    class StockItemConfig
    {
        public InventoryItem item;
        public int initialStock;
        [Range(-50, 100)]
        public float buyingDiscountPercentage;


    }

    public event Action onChange;
    public IEnumerable<ShopItem> GetFilteredItems() 
    {
        yield return new ShopItem(InventoryItem.GetFromID("0a95d347-7f66-413d-bbab-6295b018294e"), 10, 100f, 0);
        yield return new ShopItem(InventoryItem.GetFromID("283dacb9-620b-43ba-9850-b1223d0c21a7"), 1, 2000f, 0);
        yield return new ShopItem(InventoryItem.GetFromID("9195e44a-7c52-416b-a280-866391225e3c"), 1, 578.98f, 0);
        yield return new ShopItem(InventoryItem.GetFromID("9f536480-7cb0-4a18-bf7b-e2e15fff51e1"), 1, 4.50f, 0);
    }

    public void SelectFilter(ItemCategory category) { }
    public ItemCategory GetItemFilter() { return ItemCategory.None; }
    public void SelectMode(bool isBuying) { }
    public bool IsBuyingMode() { return true; }
    public bool CanTransact() { return true; }
    public float TransactionTotal() { return 0; }
    public void AddToTransaction(InventoryItem item, int quantity) { }
    public void ConfirmTransaction() { }

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

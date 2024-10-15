using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopRowUI : MonoBehaviour
{
    [SerializeField] Image iconField;
    [SerializeField] TextMeshProUGUI nameField;
    [SerializeField] TextMeshProUGUI availabilityField;
    [SerializeField] TextMeshProUGUI priceField;
    [SerializeField] TextMeshProUGUI quantityField;

    Shop currentShop;
    ShopItem item = null;

    public void Setup(Shop currentShop, ShopItem item)
    {
        this.currentShop = currentShop;
        this.item = item;
        nameField.text = item.GetName();
        iconField.sprite = item.GetIcon();
        availabilityField.text = item.GetAvailability().ToString();
        priceField.text = $"${item.GetPrice():N2}";
        quantityField.text = item.GetQuantityInTransaction().ToString();
    }

    public void Add()
    {
        currentShop.AddToTransaction(item.GetInventoryItem(), 1);
    }

    public void Remove()
    {
        currentShop.AddToTransaction(item.GetInventoryItem(), -1);
    }

}

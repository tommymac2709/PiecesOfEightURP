using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI shopName;
    [SerializeField] TextMeshProUGUI totalPriceField;
    [SerializeField] Transform listRoot;
    [SerializeField] ShopRowUI rowPrefab;
    [SerializeField] Button confirmButton;


    Shopper shopper = null;
    Shop currentShop = null;

    Color originalTotalTextColour;

    // Start is called before the first frame update
    void Start()
    {
        originalTotalTextColour = totalPriceField.color;
        shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
        if (shopper == null) { return; }

        shopper.activeShopChange += ShopChanged;

        confirmButton.onClick.AddListener(ConfirmTransaction);

        ShopChanged();
    }

    private void ShopChanged()
    {
        if (currentShop != null)
        {
            currentShop.onChange -= RefreshUI;
        }
        currentShop = shopper.GetActiveShop();
        gameObject.SetActive(currentShop != null);

        if (currentShop == null) return;

        shopName.text = currentShop.GetShopName();

        currentShop.onChange += RefreshUI;

        RefreshUI();
    }

    private void RefreshUI()
    {
        foreach (Transform child in listRoot)
        {
            Destroy(child.gameObject);
        }

        foreach (ShopItem item in currentShop.GetFilteredItems())
        {
            ShopRowUI row = Instantiate<ShopRowUI>(rowPrefab, listRoot);
            row.Setup(currentShop, item);
        }

        totalPriceField.text = $"Total: ${currentShop.TransactionTotal():N2}";
        totalPriceField.color = currentShop.HasSufficientFunds() ? originalTotalTextColour : Color.red;
        confirmButton.interactable = currentShop.CanTransact();
    }

    public void Close()
    {
        shopper.SetActiveShop(null);
    }

    public void ConfirmTransaction()
    {
        currentShop.ConfirmTransaction();
    }
}

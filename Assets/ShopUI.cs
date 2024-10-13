using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI shopName;
    [SerializeField] Transform listRoot;
    [SerializeField] ShopRowUI rowPrefab;


    Shopper shopper = null;
    Shop currentShop = null;
    // Start is called before the first frame update
    void Start()
    {
       shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
        if (shopper == null) { return; }

        shopper.activeShopChange += ShopChanged;

        ShopChanged();
    }

    private void ShopChanged()
    {
        currentShop = shopper.GetActiveShop();
        gameObject.SetActive(currentShop != null);

        if (currentShop == null) return;

        shopName.text = currentShop.GetShopName();

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
            row.Setup(item);
        }
    }

    public void Close()
    {
        shopper.SetActiveShop(null);
    }
}

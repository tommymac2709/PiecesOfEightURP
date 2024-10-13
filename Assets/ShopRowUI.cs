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

    public void Setup(ShopItem item)
    {
        nameField.text = item.GetName();
        iconField.sprite = item.GetIcon();
        availabilityField.text = item.GetAvailability().ToString();
        priceField.text = $"${item.GetPrice():N2}";
    }

}

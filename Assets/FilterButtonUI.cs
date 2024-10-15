using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterButtonUI : MonoBehaviour
{
    [SerializeField] ItemCategory category = ItemCategory.None;
    Button button;
    Shop currentShop;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SelectFilter);
    }

    public void SetShop(Shop currentShop)
    {
        this.currentShop = currentShop;
    }

    public void RefreshUI()
    {
        button.interactable = currentShop.GetItemFilter() != category;
    }

    private void SelectFilter()
    {
        currentShop.SelectFilter(category);
    }
}

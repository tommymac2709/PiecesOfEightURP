using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Tooltip", menuName = "Inventory System/UI/Tooltip")]
public class InventoryItemTooltipSO : ScriptableObject
{
    
    [SerializeField] public string itemName;
    [SerializeField] public string itemDescription;
    [SerializeField] public Sprite itemSprite;

}

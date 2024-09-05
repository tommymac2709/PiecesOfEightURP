using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Object", menuName = "Inventory System/Items/Consumable")]
public class ConsumableSO : ItemSO
{
    [SerializeField] float restoreHealthValue;
    [SerializeField] float restoreStaminaValue;

    private void Awake()
    {
        type = ItemType.Consumable;
    }
}

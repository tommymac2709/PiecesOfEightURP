using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentSO : ItemSO
{
    [SerializeField] float attackBonus;
    [SerializeField] float defenceBonus;

    private void Awake()
    {
        type = ItemType.Equipment;
    }
}

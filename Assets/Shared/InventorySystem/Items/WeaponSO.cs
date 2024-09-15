using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory System/Items/Weapon")]
public class WeaponSO : ItemSO
{
    private void Awake()
    {
        type = ItemType.Weapon;
    }
}

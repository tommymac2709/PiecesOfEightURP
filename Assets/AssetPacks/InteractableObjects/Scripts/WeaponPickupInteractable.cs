using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickupInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactText;
    [SerializeField] WeaponConfig _weapon = null;

    [SerializeField] ItemSO item;
    [SerializeField] int numberToAdd;

    public void Interact(Transform transform)
    {
        
        InventorySO inventory = GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>().Inventory;
        inventory.AddItem(new Item(item), numberToAdd);

        Fighter player = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();

        // player.EquipWeapon(_weapon);
        player.EquipWeapon(item.weaponConfig);

        Destroy(gameObject);
    }

    public string GetInteractText()
    {
        return item.interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }

}

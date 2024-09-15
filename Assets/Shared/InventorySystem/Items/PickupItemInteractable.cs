using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItemInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] ItemSO item;
    [SerializeField] int numberToAdd;

    public void EnableOutline()
    {
        throw new System.NotImplementedException();
    }

    public string GetInteractText()
    {
        return item.interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact(Transform interactorTransform)
    {
        Destroy(gameObject);
        InventorySO inventory = GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>().Inventory;
        inventory.AddItem(new Item(item), numberToAdd);
        //DisplayInventory display = GameObject.FindObjectOfType<DisplayInventory>();
        //display.UpdateDisplay();

    }

    
}

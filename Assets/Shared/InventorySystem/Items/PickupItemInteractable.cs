using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItemInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] ItemSO item;
    [SerializeField] int numberToAdd;

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
        InventorySO inventory = GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>().Inventory;
        inventory.AddItem(item, numberToAdd);
        Destroy(gameObject);
    }

    
}

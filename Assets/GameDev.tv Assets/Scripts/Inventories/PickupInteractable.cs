using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pickup))]
public class PickupInteractable : MonoBehaviour, IInteractable
{
    Pickup pickup;

    private void Awake()
    {
        pickup = GetComponent<Pickup>();    
    }

    public string GetInteractText()
    {
        return null;
    }

    public Transform GetTransform()
    {
        return pickup.transform;
    }

    public void Interact(Transform interactorTransform)
    {
        pickup.PickupItem();
    }

    
}

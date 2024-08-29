using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickupInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactText;
    [SerializeField] Weapon _weapon = null;

    public void Interact(Transform transform)
    {
        Fighter player = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();

        player.EquipWeapon(_weapon);

        Destroy(gameObject);
    }

    public string GetInteractText()
    {
        return _interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}

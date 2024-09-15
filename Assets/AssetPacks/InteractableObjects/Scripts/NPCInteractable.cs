using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactText;

    public void Interact(Transform transform)
    {
        Debug.Log("Hello, there!");
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

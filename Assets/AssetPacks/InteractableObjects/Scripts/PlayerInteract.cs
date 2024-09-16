using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float _interactRange;

    [SerializeField] private PlayerStateMachine stateMachine;

    [SerializeField] float coveredAngle = 90f;

    private void OnEnable()
    {
        stateMachine.InputReader.InteractEvent += OnInteract;
    }

    

    private void OnDisable()
    {
        stateMachine.InputReader.InteractEvent -= OnInteract;
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.E)) //checks for input of the E key
    //    {
    //        IInteractable interactable = GetInteractableObject();
    //        if (interactable != null)
    //        {
    //            interactable.Interact(transform);
    //        }
    //    }
    //}

    private void OnInteract()
    {
        IInteractable interactable = GetInteractableObject();
        if (interactable != null)
        {
            interactable.Interact(transform);
        }
    }

    private void OnDrawGizmos()
    {
        
    }

    public IInteractable GetInteractableObject()
    {
        List<IInteractable> interactableList = new List<IInteractable>();
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, _interactRange); //returns an ARRAY of COLLIDERS
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out IInteractable interactable) && PickupInCoverage(interactable))
            {
                interactableList.Add(interactable);
                
            }
        }

        IInteractable closestInteractable = null;
        foreach(IInteractable interactable in interactableList)
        {
            if (closestInteractable == null)
            {
                closestInteractable= interactable;
            }
            else
            {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) < Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
                {
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;


    }

    private bool PickupInCoverage(IInteractable interactable)
    {
        var requiredValue = Mathf.Cos(coveredAngle * Mathf.Deg2Rad);
        GameObject interactableObject = interactable.GetTransform().gameObject;
        var directionToInteractable = (interactableObject.transform.position - transform.position).normalized;
        var dotProduct = Vector3.Dot(transform.forward, directionToInteractable);
        return dotProduct >= requiredValue;
    }

}

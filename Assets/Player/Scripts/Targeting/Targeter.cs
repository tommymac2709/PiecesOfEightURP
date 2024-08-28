using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;

    public List<Target> targets = new List<Target>();

    public Target CurrentTarget { get; private set; }

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }

        targets.Add(target);
        target.OnDestroyed += RemoveTarget;

    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.TryGetComponent<Target>(out Target target)) { return; }

        RemoveTarget(target);

    }

    public bool SelectTarget()
    {
        if (targets.Count == 0) return false;

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;   

        foreach (Target target in targets) 
        {
            Vector3 viewPos = mainCamera.WorldToViewportPoint(target.transform.position); //CHANGE TO VECTOR2 FOR CLOSEST TO CENTRE OF SCREEN TARGETING

            if (!target.GetComponentInChildren<Renderer>().isVisible)
            {
                continue;
            }

            Vector3 toPlayer = viewPos - transform.position;

            //Vector2 toCentre = viewPos - new Vector2(0.5f, 0.5f); UNCOMMENT FOR CLOSEST TO CENTRE OF SCREEN TARGETING 
            if (toPlayer.sqrMagnitude < closestTargetDistance) //change toPlayer to toCentre FOR CLOSEST TO CENTRE OF SCREEN TARGETING
            { 
                closestTarget = target;
                closestTargetDistance = toPlayer.sqrMagnitude; //change toPlayer to toCentre FOR CLOSEST TO CENTRE OF SCREEN TARGETING
            }
        }

        if (closestTarget == null) return false;

        CurrentTarget = closestTarget;
        cinemachineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);


        return true;

    }

    public void CancelTargeting()
    {
        if (CurrentTarget == null) return;

        cinemachineTargetGroup.RemoveMember(CurrentTarget.transform);

        CurrentTarget = null;   
    }

    private void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            cinemachineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }
}

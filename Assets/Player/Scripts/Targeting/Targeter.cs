using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;
using System.Linq;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;

    [Tooltip("The higher the value, the more precedence will be given to how close the target is to the targeter." +
                 "The lower the value, the more precedence will be given to how close the target is to the center of the screen.")]
    [Range(0, 1)]
    [SerializeField] private float distanceWeight = .5f;


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
        Rect bounds = new Rect(0, 0, 1, 1);
        Target closestTarget = null;
        float ClosestDistanceToCenter = Mathf.Infinity;
        foreach (Target target in targets.OrderByDescending(t => Vector3.SqrMagnitude(t.transform.position - transform.position)))
        {
            Target candidate = null;
            float candidateToCenter = Mathf.Infinity;
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);
            if (!bounds.Contains(viewPos)) continue;
            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
            float distanceToTarget = toCenter.sqrMagnitude;
            if (distanceToTarget < ClosestDistanceToCenter)
            {
                candidate = target;
            }
            else if (closestTarget != null)
            {
                float distanceOfCandidateToCamera = Vector3.SqrMagnitude(target.transform.position - transform.position);
                float distanceOfClosestToCamera =
                    Vector3.SqrMagnitude(closestTarget.transform.position - transform.position);
                if (distanceOfCandidateToCamera > distanceOfClosestToCamera * distanceWeight)
                {
                    continue;
                }
                candidate = target;
            }
            closestTarget = candidate;
            ClosestDistanceToCenter = distanceToTarget;
        }
        if (closestTarget == null) return false;
        CurrentTarget = closestTarget;
        cinemachineTargetGroup.AddMember(CurrentTarget.transform, 1, 2);
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

    public Target GetTarget()
    {
        return CurrentTarget;
    }
}

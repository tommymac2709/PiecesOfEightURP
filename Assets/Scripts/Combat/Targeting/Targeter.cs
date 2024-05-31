using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Targeter : MonoBehaviour
{

    public List<Target> targets = new List<Target>();

    public Target CurrentTarget { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }

        targets.Add(target);

    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.TryGetComponent<Target>(out Target target)) { return; }

        targets.Remove(target);

    }

    public bool SelectTarget()
    {
        if (targets.Count == 0) return false;

        CurrentTarget = targets[0];

        return true;

    }

    public void CancelTargeting()
    {
        CurrentTarget = null;   
    }
}

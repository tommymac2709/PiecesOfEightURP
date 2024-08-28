using System.Collections;
using UnityEngine;
using System;
public abstract class Ability : ScriptableObject
{
    public string abilityName;
    public float cooldownTime;
    public float activeTime;
    public bool isCoolingDown { get; private set; } = false;
    public bool isActive { get; private set; } = false;

    public event Action AbilityDeactivatedEvent;

    

    public virtual void Activate(GameObject parent)
    {
        if (isActive) { return; }
        Debug.Log(abilityName + " activated.");
        // Add custom activation logic here
        isActive = true;
        
        parent.GetComponent<MonoBehaviour>().StartCoroutine(DeactivateAfterTime(parent));
    }

    public virtual void Deactivate(GameObject parent)
    {
        Debug.Log(abilityName + " deactivated.");
        isActive = false;
        
        parent.GetComponent<MonoBehaviour>().StartCoroutine(Cooldown());
        
        // Add custom deactivation logic here
    }

    protected void OnAbilityDeactivated()
    {
        AbilityDeactivatedEvent?.Invoke();
    }

    private IEnumerator Cooldown()
    {
        isCoolingDown = true;
        OnAbilityDeactivated();
        yield return new WaitForSeconds(cooldownTime);
        isCoolingDown = false;
    }

    private IEnumerator DeactivateAfterTime(GameObject parent)
    {
        yield return new WaitForSeconds(activeTime);
        Deactivate(parent);
    }

    public virtual bool RequireTargeting()
    {
        return false;
    }

    public virtual bool CanMove()
    {
        return false;
    }

}

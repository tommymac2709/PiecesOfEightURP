using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Invisibility")]
public class Invisibility : Ability
{
    public override void Activate(GameObject parent)
    {
        base.Activate(parent);
        parent.GetComponent<AbilityManager>().IsInvisible = true;
        // Specific logic for Invisibility
        Renderer[] renderer = parent.GetComponentsInChildren<Renderer>();
        if (renderer != null)
        {
            foreach (Renderer r in renderer)
            {
                r.enabled = false;
            }
            
        }
    }

    public override void Deactivate(GameObject parent)
    {
        base.Deactivate(parent);
        parent.GetComponent<AbilityManager>().IsInvisible = true;
        // Logic to deactivate invisibility
        Renderer[] renderer = parent.GetComponentsInChildren<Renderer>();
        if (renderer != null)
        {
            foreach (Renderer r in renderer)
            {
                r.enabled = true;
            }

        }
    }

    public override bool CanMove()
    {
        return true;
    }
}
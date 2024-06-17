using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Invisibility")]
public class Invisibility : Ability
{
    public override void Activate(GameObject parent)
    {
        base.Activate(parent);
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
}
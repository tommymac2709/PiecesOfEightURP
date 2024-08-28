using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ForcePush")]
public class ForcePush : Ability
{
    public override void Activate(GameObject parent)
    {
        base.Activate(parent);

        Debug.Log("Force PUSH");
    }

    public override void Deactivate(GameObject parent)
    {
        base.Deactivate(parent);
        
    }
}
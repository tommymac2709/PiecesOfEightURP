using UnityEditor.Playables;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ForcePull")]
public class ForcePull : Ability
{
    public bool requiresTargeting = true;
    private readonly int ForcePullHash = Animator.StringToHash("ForcePull");
    private const float CrossFadeDuration = 0.1f;
    public override void Activate(GameObject parent)
    {
        base.Activate(parent);
        PlayerStateMachine stateMachine = parent.GetComponent<PlayerStateMachine>();
        stateMachine.Animator.CrossFadeInFixedTime(ForcePullHash, CrossFadeDuration);

        Targeter targeter = parent.GetComponentInChildren<Targeter>();
        if (targeter == null) 
        {
            return;
        }
        Target target = targeter.CurrentTarget;
        EnemyStateMachine enemyStateMachine = target.GetComponent<EnemyStateMachine>();


        Debug.Log("Force PULL");
        Debug.Log(target.name);

        enemyStateMachine.SwitchState(new EnemyForcePulledState(enemyStateMachine));
        
    }

    public override void Deactivate(GameObject parent)
    {
        base.Deactivate(parent);

    }

    public override bool RequireTargeting()
    {
        return true;
    }
    public override bool CanMove()
    {
        return false;
    }


}
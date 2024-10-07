using UnityEngine;
using UnityEngine.AI;

public class EnemyDeadState : EnemyBaseState
{
    private readonly int DeathAnimHash = Animator.StringToHash("Death");
    
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(DeathAnimHash, CrossFadeDuration);
        stateMachine.Controller.enabled = false;
        stateMachine.Animator.enabled = false;
        //var navMesh = stateMachine.gameObject.GetComponent<NavMeshAgent>();
        //navMesh.enabled = false;
        stateMachine.enabled = false;

        //stateMachine.Ragdoll.ToggleRagdoll(true);
        //stateMachine.WeaponDamage.gameObject.SetActive(false);
        GameObject.Destroy(stateMachine.Target);
        
    }

    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {

    }


}

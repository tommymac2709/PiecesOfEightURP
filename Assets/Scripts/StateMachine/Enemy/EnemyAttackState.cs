using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private readonly int AttackAnimHash = Animator.StringToHash("Attack");
    
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(AttackAnimHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
       
    }

    public override void Exit()
    {
        
    }

    
}
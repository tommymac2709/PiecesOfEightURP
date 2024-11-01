using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    private readonly int impactAnimHash;
    private const float CrossFadeDuration = 0.1f;
    private AttackData attackData;

    public EnemyImpactState(EnemyStateMachine stateMachine, AttackData attackData) : base(stateMachine)
    {
        impactAnimHash = Animator.StringToHash(attackData.ImpactAnimationName);
        this.attackData = attackData;
    }

    public override void Enter()
    {
        
        if (attackData.ApplyImpactRootMotion) stateMachine.Animator.applyRootMotion = true;

        stateMachine.Animator.CrossFadeInFixedTime(impactAnimHash, CrossFadeDuration);

        Debug.Log(stateMachine.Health.currentHealth);
    }
    public override void Tick(float deltaTime)
    {
        MoveNoInput(deltaTime);
        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Impact");
        //stateDuration -= deltaTime;

        //if (stateDuration <= 0f)
        //{
        //    stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        //}
        if (normalizedTime > 0.9f)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }

    }

    public override void Exit()
    {
        stateMachine.Animator.applyRootMotion = false;

    }


}

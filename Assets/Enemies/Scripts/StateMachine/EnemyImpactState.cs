using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    private readonly int ImpactAnimHash = Animator.StringToHash("Impact");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    private float stateDuration;

    public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override void Enter()
    {
        
        stateMachine.Animator.CrossFadeInFixedTime(ImpactAnimHash, CrossFadeDuration);
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

    }


}

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
        stateDuration = stateMachine.ImpactStateDuration;
        stateMachine.Animator.CrossFadeInFixedTime(ImpactAnimHash, CrossFadeDuration);
    }
    public override void Tick(float deltaTime)
    {
        MoveNoInput(deltaTime);

        stateDuration -= deltaTime;

        if (stateDuration <= 0f)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }

    }

    public override void Exit()
    {

    }


}

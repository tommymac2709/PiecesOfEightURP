using UnityEngine;

public class EnemyParriedState : EnemyBaseState
{
    private readonly int ParriedAnimHash = Animator.StringToHash("Parried");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    private float stateDuration;

    public EnemyParriedState(EnemyStateMachine stateMachine, float stateDuration) : base(stateMachine)
    {
        this.stateDuration = stateDuration;
    }

    public override void Enter()
    {

        stateMachine.Animator.CrossFadeInFixedTime(ParriedAnimHash, CrossFadeDuration);
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

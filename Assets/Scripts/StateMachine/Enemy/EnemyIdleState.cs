using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int MotionBlendTreeHash = Animator.StringToHash("Motion");
    private readonly int MovementSpeedHash = Animator.StringToHash("MovementSpeed");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        
        stateMachine.Animator.CrossFadeInFixedTime(MotionBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        MoveNoInput(deltaTime);

        if (IsInChaseRange())
        {

            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        //if (IsInFleeRange())
        //{

        //    stateMachine.SwitchState(new EnemyFleeState(stateMachine));
        //    return;
        //}

        stateMachine.Animator.SetFloat(MovementSpeedHash, 0, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        
    }

    

    
}

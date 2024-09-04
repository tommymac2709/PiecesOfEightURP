using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        
        stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        MoveNoInput(deltaTime);
        
        //if (!CanSeePlayer())
        //{
        //    return;
        //}

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

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        
    }

    

    
}

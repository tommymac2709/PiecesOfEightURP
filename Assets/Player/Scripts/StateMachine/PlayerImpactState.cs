using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    private readonly int ImpactAnimHash = Animator.StringToHash("Impact");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    private float stateDuration;

    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactAnimHash, CrossFadeDuration);
        stateDuration = stateMachine.ImpactStateDuration;
    }
    public override void Tick(float deltaTime)
    {
        MoveNoInput(deltaTime);

        stateDuration -= deltaTime;

        if (stateDuration <= 0f)
        {
            ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
        
    }

    
}

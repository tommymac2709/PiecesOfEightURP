using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    private readonly int BlockAnimHash = Animator.StringToHash("Block");

    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.1f;
    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(BlockAnimHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit()
    {
        
    }

    
}

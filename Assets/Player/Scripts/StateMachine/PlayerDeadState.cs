using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    private readonly int DeathAnimHash = Animator.StringToHash("Death");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(DeathAnimHash, CrossFadeDuration);

        stateMachine.Health.onResurrection.Invoke();
    }



    public override void Tick(float deltaTime)
    {
        MoveNoInput(deltaTime);
    }

    public override void Exit()
    {
        
    }

    
}

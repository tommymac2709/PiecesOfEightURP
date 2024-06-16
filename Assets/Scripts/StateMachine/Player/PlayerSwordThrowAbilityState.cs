using UnityEngine;

public class PlayerSwordThrowAbilityState : PlayerBaseState
{
    private readonly int SwordThrowHash = Animator.StringToHash("SwordThrow");

    private readonly int SwordCatchHash = Animator.StringToHash("SwordCatch");

    private const float CrossFadeDuration = 0.1f;

    public PlayerSwordThrowAbilityState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(SwordThrowHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {
        
    }

    

    
}

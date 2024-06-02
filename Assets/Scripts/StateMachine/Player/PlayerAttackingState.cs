using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private AttackData attackData;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackID) : base(stateMachine)
    {
        attackData = stateMachine.AttackData[attackID];
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(attackData.animationName, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit()
    {
        
    }

    
}

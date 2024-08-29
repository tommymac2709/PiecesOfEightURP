using System;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    private readonly int BlockAnimHash = Animator.StringToHash("Block");

    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.1f;

    private float parryTimer = 0.2f;

    



    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entered blocking state");
        stateMachine.DamageReceiver.SetCanParry(true);
        stateMachine.DamageReceiver.SetIsBlocking(true);
        stateMachine.Animator.CrossFadeInFixedTime(BlockAnimHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        parryTimer -= deltaTime;
        if (parryTimer <= 0) 
        { 
            stateMachine.DamageReceiver.SetCanParry(false);
        
        }

        MoveNoInput(deltaTime);

        if (!stateMachine.InputReader.IsBlocking || stateMachine.InputReader.IsTargeting && stateMachine.Targeter.CurrentTarget == null)
        {
            ReturnToLocomotion();
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.DamageReceiver.SetCanParry(false);
        stateMachine.DamageReceiver.SetIsBlocking(false);
    }

    
}

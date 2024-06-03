using System;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float previousFrameTime;

    private AttackData attackData;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attackData = stateMachine.AttackData[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(attackData.AnimationName, attackData.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        MoveNoInput(deltaTime);

        FaceTarget();


        float normalizedTime = GetNormalizedTime();

        if (normalizedTime > previousFrameTime && normalizedTime < 1f)
        {

            if(normalizedTime >= attackData.ForceTime)
            {
                TryApplyForce();
            }

            if (stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            // go back to locomotion
        }

        previousFrameTime = normalizedTime;
    }

    
    public override void Exit()
    {
        
    }
    private void TryComboAttack(float normalizedTime)
    {
        if (attackData.ComboStateIndex == -1) { return; }

        if (normalizedTime < attackData.ComboAttackTime) { return; }

        stateMachine.SwitchState(new PlayerAttackingState(stateMachine,attackData.ComboStateIndex));
    }

    private void TryApplyForce()
    {

    }

    private float GetNormalizedTime()
    {
        AnimatorStateInfo currentStateInfo =  stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextStateInfo = stateMachine.Animator.GetNextAnimatorStateInfo(0);

        if (stateMachine.Animator.IsInTransition(0) && nextStateInfo.IsTag("Attack"))
        {
            return nextStateInfo.normalizedTime;
        }
        else if (!stateMachine.Animator.IsInTransition(0) && currentStateInfo.IsTag("Attack"))
        {
            return currentStateInfo.normalizedTime;
        }
        else
        {
            return 0f;  
        }
    }

    
}

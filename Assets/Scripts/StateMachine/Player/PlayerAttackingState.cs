using System;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private bool alreadyAppliedForce = false;

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
        Vector3 movement = CalculateMovement();

        if (normalizedTime >= previousFrameTime && normalizedTime < 1f)
        {

            if(normalizedTime >= attackData.ForceTime)
            {
                TryApplyForce();
            }

            if (stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);

                if (stateMachine.Targeter.CurrentTarget != null && stateMachine.InputReader.IsTargeting) return;

                if (movement == Vector3.zero) { return; }

                FaceDirection(movement, Time.deltaTime);
            }
        }
        else
        {
            if (stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
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
        if (alreadyAppliedForce) { return; }

        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attackData.Force);

        alreadyAppliedForce = true;
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

    private Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y + right * stateMachine.InputReader.MovementValue.x;
    }

    private void FaceDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), deltaTime * stateMachine.RotationDamping);
    }

    
}

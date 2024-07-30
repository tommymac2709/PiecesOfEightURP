using RPGCharacterAnims.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");

    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");

    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.1f;

    private bool shouldFade;

    public PlayerFreeLookState(PlayerStateMachine stateMachine, bool shouldFade = true) : base(stateMachine) 
    { 
        this.shouldFade = shouldFade;
    
    }

    public override void Enter()
    {
        stateMachine.FreeLookCamera.SetActive(true);
        stateMachine.TargetCamera.SetActive(false);
        
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.UseAbilityEvent += OnUseAbility;

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0f);

        if (shouldFade)
        {
            stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
        }
        else
        {
            stateMachine.Animator.Play(FreeLookBlendTreeHash);
        }
        
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        if (stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();

        if (stateMachine.InputReader.IsSprinting)
        {
            Move(movement * stateMachine.FreeLookSprintMovementSpeed, deltaTime); //Calls from BaseState
        }
        else
        {
            Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime); //Calls from BaseState
        }
        

        

        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);

        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.UseAbilityEvent -= OnUseAbility;
    }

    private void OnUseAbility()
    {
        if (stateMachine.AbilityManager.abilities[stateMachine.AbilityManager.currentAbilityIndex].isCoolingDown ) { return; }
        if (stateMachine.AbilityManager.abilities[stateMachine.AbilityManager.currentAbilityIndex].RequireTargeting()) { return; }
        stateMachine.SwitchState(new PlayerUseAbilityState(stateMachine, stateMachine.AbilityManager.abilities[stateMachine.AbilityManager.currentAbilityIndex]));
        return;
    }

    private void OnDodge()
    {
        if(stateMachine.InputReader.MovementValue.x != 0 || stateMachine.InputReader.MovementValue.y != 0)
        {
            stateMachine.SwitchState(new PlayerFreeLookDodgeState(stateMachine));
            return;
        }
        
    }

    private void OnJump() 
    { 
        stateMachine.SwitchState(new PlayerJumpState(stateMachine));
        return;
    }

    private void OnTarget()
    {
        if (!stateMachine.Targeter.SelectTarget()) return;
        
        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        return;
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y +
            right * stateMachine.InputReader.MovementValue.x;
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
            stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationDamping);
    }
}

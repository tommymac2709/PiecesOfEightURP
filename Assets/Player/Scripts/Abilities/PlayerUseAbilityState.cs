using System;
using UnityEngine;

public class PlayerUseAbilityState : PlayerBaseState
{
    private Ability ability;
    
    public PlayerUseAbilityState(PlayerStateMachine stateMachine, Ability ability) : base(stateMachine)
    {
        this.ability = ability;
    }

    public override void Enter()
    {
        stateMachine.InputReader.UseAbilityEvent -= OnUseAbility; // Prevent triggering another ability
        ability.Activate(stateMachine.gameObject);
        ability.AbilityDeactivatedEvent += OnAbilityDeactivated;
        
    }

    private void OnUseAbility()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();


        if (ability.CanMove()) 
        {
            Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);
            FaceMovementDirection(movement, deltaTime);
        }
        else
        {
            MoveNoInput(deltaTime);
        }

        
        // You can add any updates required during the ability's active time here
    }

    public override void Exit()
    {
        Debug.Log("Exited UseAbulity State");
        ability.AbilityDeactivatedEvent -= OnAbilityDeactivated;
        
        //ability.Deactivate(stateMachine.gameObject);
        
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

    private void OnAbilityDeactivated()
    {
        Debug.Log("Returned to Locomotion");
        ReturnToLocomotion();
    }
}

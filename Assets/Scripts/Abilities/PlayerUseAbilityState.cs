using System;

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
        // You can add any updates required during the ability's active time here
    }

    public override void Exit()
    {
        ability.AbilityDeactivatedEvent -= OnAbilityDeactivated;
        
        //ability.Deactivate(stateMachine.gameObject);
        
    }

    private void OnAbilityDeactivated()
    {
        ReturnToLocomotion();
    }
}

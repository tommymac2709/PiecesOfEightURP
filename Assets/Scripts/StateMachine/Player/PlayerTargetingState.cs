//using System.Diagnostics;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;
        Debug.Log("Entered Targeting State");
    }

    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {
        Debug.Log("Exited Targeting State");
        stateMachine.InputReader.TargetEvent -= OnTarget;
    }

    private void OnTarget()
    {
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    
}

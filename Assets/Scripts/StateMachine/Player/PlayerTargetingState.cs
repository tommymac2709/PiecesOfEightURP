//using System.Diagnostics;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");


    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.TargetCamera.SetActive(true);
        stateMachine.FreeLookCamera.SetActive(false);

        stateMachine.InputReader.TargetEvent += OnTarget;

        stateMachine.Animator.Play(TargetingBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
            //This return stops anything else under it from occuring in the state
        }
    }

    public override void Exit()
    {
        Debug.Log("Exited Targeting State");
        stateMachine.InputReader.TargetEvent -= OnTarget;
    }

    private void OnTarget()
    {
        stateMachine.Targeter.CancelTargeting();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    
}

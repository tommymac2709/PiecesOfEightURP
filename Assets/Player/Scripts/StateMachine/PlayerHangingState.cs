using Unity.VisualScripting;
using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    private readonly int HangAnimHash = Animator.StringToHash("LedgeIdle");
    private readonly int GrabAnimHash = Animator.StringToHash("LedgeGrab");

    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.1f;

    private Vector3 closestPoint;
    private Vector3 ledgeForward;

    private float stateTimer = 0f;
    public PlayerHangingState(PlayerStateMachine stateMachine, Vector3 closestPoint, Vector3 ledgeForward) : base(stateMachine)
    {
        this.closestPoint = closestPoint;
        this.ledgeForward = ledgeForward;
    }

    public override void Enter()
    {
        
        stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward, Vector3.up);
        
        stateMachine.Animator.CrossFadeInFixedTime(HangAnimHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    { 
        stateTimer += deltaTime;

        if (stateTimer > 0.25f &&  stateMachine.InputReader.MovementValue.y > 0f)
        {
            
            stateMachine.SwitchState(new PlayerPullUpState(stateMachine));
        }
        else if (stateMachine.InputReader.MovementValue.y < 0f) 
        {
            stateMachine.Controller.Move(Vector3.zero);
            stateMachine.ForceReceiver.Reset();
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
        }

        
    }

    public override void Exit()
    {
        
    }

    

}

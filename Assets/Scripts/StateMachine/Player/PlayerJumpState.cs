using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private readonly int JumpAnimHash = Animator.StringToHash("Jump");

    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.1f;

    private Vector3 momentum;


    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);

        momentum = stateMachine.Controller.velocity / 2;
        momentum.y = 0f;

        stateMachine.Animator.CrossFadeInFixedTime(JumpAnimHash, CrossFadeDuration);
    }
    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if (stateMachine.Controller.velocity.y <= 0) 
        {
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
            return;
        }

        FaceTarget(); //if we jump in freelook state this will be ignored as it checks for a target
    }

    public override void Exit()
    {
        
    }

    
}

using UnityEngine;

public class PlayerPullUpState : PlayerBaseState
{
    private readonly int LedgeClimbAnimHash = Animator.StringToHash("LedgeClimb");

    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.1f;

    private readonly Vector3 offset = new Vector3(0f, 3.5f, 2f);

    public PlayerPullUpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ClimbCamera.SetActive(true);
        stateMachine.FreeLookCamera.SetActive(false);
        stateMachine.Animator.CrossFadeInFixedTime(LedgeClimbAnimHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator, "Climbing") < 1f) { return; }

        stateMachine.Controller.enabled = false;
        stateMachine.transform.Translate(offset, Space.Self);
        stateMachine.Controller.enabled = true;

        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine, false));
    }

    public override void Exit()
    {
        stateMachine.FreeLookCamera.SetActive(true);
        stateMachine.ClimbCamera.SetActive(false);
        
        stateMachine.Controller.Move(Vector3.zero);
        stateMachine.ForceReceiver.Reset();
    }

    
}

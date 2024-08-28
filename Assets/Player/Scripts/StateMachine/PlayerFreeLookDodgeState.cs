using UnityEngine;

public class PlayerFreeLookDodgeState : PlayerBaseState
{
    
    private readonly int DodgeRollHash = Animator.StringToHash("DodgeRoll");

    private const float CrossFadeDuration = 0.1f;

    private float invulnerabilityTimer;
    private float remainingDodgeDuration;
    Vector3 movement;


    public PlayerFreeLookDodgeState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override void Enter()
    {
        stateMachine.DamageReceiver.SetIsInvulnerable(true);
        invulnerabilityTimer = stateMachine.DodgeInvulnerabilityDurationFreeLook;

        remainingDodgeDuration = stateMachine.DodgeDurationFreeLook;

        stateMachine.Animator.CrossFadeInFixedTime(DodgeRollHash, CrossFadeDuration);
        Vector3 movement = CalculateMovement();


        movement += movement * stateMachine.DodgeDistanceFreeLook / stateMachine.DodgeDurationFreeLook;

        this.movement = movement;

        

    }

    public override void Tick(float deltaTime)
    {

        Move(movement, Time.deltaTime);
        FaceMovementDirection(movement, Time.deltaTime);
        remainingDodgeDuration -= deltaTime;
        invulnerabilityTimer -= deltaTime;

        if (invulnerabilityTimer <= 0) 
        {
            stateMachine.DamageReceiver.SetIsInvulnerable(false);
        }

        if (remainingDodgeDuration <= 0)
        {
            ReturnToLocomotion();
        }

    }

    public override void Exit()
    {

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

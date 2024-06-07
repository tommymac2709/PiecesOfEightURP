using UnityEngine;

public class PlayerFreeLookDodgeState : PlayerBaseState
{
    
    private readonly int DodgeRollHash = Animator.StringToHash("DodgeRoll");

    private const float CrossFadeDuration = 0.1f;

    
    private float remainingDodgeDuration;
    


    public PlayerFreeLookDodgeState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override void Enter()
    {
        remainingDodgeDuration = stateMachine.DodgeDurationFreeLook;

        stateMachine.Animator.CrossFadeInFixedTime(DodgeRollHash, CrossFadeDuration);
        

    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();


        movement +=  movement * stateMachine.DodgeDistanceFreeLook / stateMachine.DodgeDurationFreeLook;

        Move(movement, deltaTime);

        FaceMovementDirection(movement, deltaTime);


        remainingDodgeDuration -= deltaTime;

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

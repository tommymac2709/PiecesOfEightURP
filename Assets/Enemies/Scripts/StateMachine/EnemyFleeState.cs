using UnityEngine;

public class EnemyFleeState : EnemyBaseState
{

    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public EnemyFleeState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        
        
        FleeFromPlayer(deltaTime);

        FaceMovementDirection();

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    private void FleeFromPlayer(float deltaTime)
    {
        //Vector3 dirToPlayer = stateMachine.transform.position - stateMachine.Player.transform.position;
        //Vector3 newPos = stateMachine.transform.position + dirToPlayer;
        stateMachine.Agent.SetDestination(stateMachine.transform.position + (stateMachine.transform.position - stateMachine.Player.transform.position));

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, stateMachine.MovementSpeed, AnimatorDampTime, deltaTime);
        Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);


        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    }

    

    
}

using UnityEngine;

public class EnemyWanderState : EnemyBaseState
{
    float timer;
    private readonly int MotionBlendTreeHash = Animator.StringToHash("Motion");
    private readonly int MovementSpeedHash = Animator.StringToHash("MovementSpeed");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    public EnemyWanderState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        
        timer = stateMachine.WanderTimer;
        stateMachine.Animator.CrossFadeInFixedTime(MotionBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (IsInFleeRange())
        {
            stateMachine.SwitchState(new EnemyFleeState(stateMachine));
            return;
        }

        WanderMovement(deltaTime);

        FaceMovementDirection();

        if (stateMachine.Controller.velocity == Vector3.zero)
        {
            stateMachine.Animator.SetFloat(MovementSpeedHash, 0f, AnimatorDampTime, deltaTime);
        }
        else
        {
            stateMachine.Animator.SetFloat(MovementSpeedHash, 1f, AnimatorDampTime, deltaTime);
        }
        
    }

    public override void Exit()
    {
        
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    private void WanderMovement(float deltaTime)
    {
        timer += deltaTime;

        if (timer >= stateMachine.WanderTimer)
        {
            Vector3 newPos = RandomNavSphere(stateMachine.transform.position, stateMachine.WanderRadius, -1);

            stateMachine.Agent.SetDestination(newPos);
          
            timer = 0;
        }

        Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);

        
        stateMachine.Agent.velocity = stateMachine.Controller.velocity;


        
    }

    

    
}

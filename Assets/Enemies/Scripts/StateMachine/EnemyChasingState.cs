using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        //if (!CanSeePlayer())
        //{
        //    stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        //    return;
        //}

        if (!IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }

        if (IsInAttackRange())
        {
            stateMachine.SwitchState(new EnemyAttackState(stateMachine, 0));
            return;
        }

        MoveToPlayer(deltaTime);

        FacePlayer();

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        if (stateMachine.Agent.enabled)
        {
            stateMachine.Agent.ResetPath();
            stateMachine.Agent.velocity = Vector3.zero;
        }
        
    }

    private void MoveToPlayer(float deltaTime)
    {
        if (stateMachine.Agent.enabled)
        {
            stateMachine.Agent.destination = stateMachine.Player.transform.position;

            Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);

            stateMachine.Agent.velocity = stateMachine.Controller.velocity;
        }
        else
        {
            MoveNoInput(deltaTime);
        }
        
    }
}

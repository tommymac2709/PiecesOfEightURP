using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int TargetingSpeedHash = Animator.StringToHash("TargetingRightSpeed");
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
        if (deltaTime == 0f) { return; }
        
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
        Vector3 lastPosition = stateMachine.transform.position;

        if (IsInAttackRange())
        {
            if (!IsOtherEnemyAttacking() && !stateMachine.CooldownTokenManager.HasCooldown("Attack"))
            {
                stateMachine.SwitchState(new EnemyPreAttackState(stateMachine)); // Switch to pre-attack circling
                return;
            }
            else
            {
                MoveNoInput(deltaTime);
                stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0);
                return;
            }
        }
        else
        {
            MoveToPlayer(deltaTime);
        }






        Vector3 deltaMovement = lastPosition - stateMachine.transform.position;
        float deltaMagnitude = deltaMovement.magnitude;
        float grossSpeed = deltaMagnitude / deltaTime;
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, grossSpeed / stateMachine.MovementSpeed, AnimatorDampTime, deltaTime);

        if (deltaMagnitude > 0)
        {
            FaceTarget(stateMachine.transform.position - deltaMovement, deltaTime);
        }
        else
        {
            FaceTarget(stateMachine.Player.transform.position, deltaTime);
        }


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
            Vector3 desiredVelocity = stateMachine.Agent.desiredVelocity.normalized;
            Move(desiredVelocity * stateMachine.MovementSpeed, deltaTime);
            stateMachine.Agent.velocity = stateMachine.Controller.velocity;
            stateMachine.Agent.nextPosition = stateMachine.transform.position;

        }
        else
        {
            MoveNoInput(deltaTime);
        }
        
    }

    private bool IsOtherEnemyAttacking()
    {
        Collider[] hitColliders = Physics.OverlapSphere(stateMachine.transform.position, stateMachine.AttackRange *stateMachine.AttackRange);

        foreach (Collider collider in hitColliders)
        {
            EnemyStateMachine otherEnemy = collider.GetComponent<EnemyStateMachine>();
            if (otherEnemy != null && otherEnemy != stateMachine && otherEnemy.IsAttacking)
            {
                return true; // Another enemy is attacking
            }
        }

        return false; // No other enemies are attacking
    }

}

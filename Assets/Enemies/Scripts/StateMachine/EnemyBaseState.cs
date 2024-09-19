using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void MoveNoInput(float deltaTime)
    {
        //Calls the move function below
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        Vector3 intendedMovement = (motion + stateMachine.ForceReceiver.Movement) * deltaTime;
        if (SampleNavMesh(stateMachine.transform.position + intendedMovement))
        {
            stateMachine.Controller.Move(intendedMovement);
        }
        else
        {
            stateMachine.Controller.Move(new Vector3(0, stateMachine.ForceReceiver.Movement.y, 0));
        }

    }

    protected void FaceTarget(Vector3 target, float deltaTime)
    {
        Vector3 directionToTarget = target - stateMachine.transform.position;
        directionToTarget.y = 0;
        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(directionToTarget), stateMachine.RotationSpeed * deltaTime);
    }

    protected bool SampleNavMesh(Vector3 position)
    {
        if (!stateMachine.Controller.isGrounded)
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(position, Vector3.down, out hit, 2, 1 << LayerMask.NameToLayer("Terrain"));
            if (hasHit) position = hit.point;
        }
        NavMeshHit navMeshHit;
        bool hasCastToNavMesh = NavMesh.SamplePosition(
            position, out navMeshHit, 1f, NavMesh.AllAreas);
        if (!hasCastToNavMesh) return false;
        return true;
    }

    protected void FaceMovementDirection()
    {
        Vector3 lookPos = stateMachine.Agent.destination - stateMachine.transform.position;
        lookPos.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    protected bool IsInChaseRange()
    {
        if (stateMachine.Player.IsDead) { return false; }

        float distanceToPlayerSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

        return distanceToPlayerSqr  <= stateMachine.PlayerDetectRange * stateMachine.PlayerDetectRange;
    }

    protected bool IsInAttackRange()
    {
        if (stateMachine.Player.IsDead) { return false; }

        float distanceToPlayerSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

        return distanceToPlayerSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }

    protected bool IsInFleeRange()
    {
        if (stateMachine.Player.IsDead) { return false; }

        float distanceToPlayerSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

        return distanceToPlayerSqr <= stateMachine.PlayerFleeRange * stateMachine.PlayerFleeRange;
    }

    protected bool CanSeePlayer()
    {
        if (stateMachine.PlayerAbilityManager.IsInvisible == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}

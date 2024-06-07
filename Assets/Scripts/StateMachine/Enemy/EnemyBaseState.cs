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
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void FacePlayer()
    {
        if (stateMachine.Player == null) { return; }

        Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
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

    protected Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
